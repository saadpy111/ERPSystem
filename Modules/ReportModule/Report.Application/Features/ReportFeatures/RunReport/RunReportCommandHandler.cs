using MediatR;
using Microsoft.Extensions.Configuration;
using Report.Application.Contracts.Persistence.Repositories;
using Report.Application.DTOs;
using Report.Domain.Entities;
using Report.Domain.Enums;
using SqlKata;
using System.Text.Json;

namespace Report.Application.Features.ReportFeatures.RunReport
{
    public class RunReportCommandHandler : IRequestHandler<RunReportCommandRequest, RunReportCommandResponse>
    {
        private readonly IReportsRepository _reportRepository;
        private readonly IReportQueryEngine _queryEngine;
        private readonly IConfiguration _configuration;

        public RunReportCommandHandler(IReportsRepository reportRepository, IReportQueryEngine queryEngine, IConfiguration configuration)
        {
            _reportRepository = reportRepository;
            _queryEngine = queryEngine;
            _configuration = configuration;
        }

        public async Task<RunReportCommandResponse> Handle(RunReportCommandRequest request, CancellationToken cancellationToken)
        {
            #region Fetch Report
            var report = await _reportRepository.GetFullReportAsync(request.ReportId);
            if (report == null) throw new KeyNotFoundException($"Report {request.ReportId} not found.");
            #endregion

            #region Execute Main Query
            var query = BuildQuery(report, request);
            var connectionString = _configuration?.GetConnectionString("ConnectionString") ?? "";
            var rows = await _queryEngine.ExecuteAsync(query, connectionString);
            #endregion

            #region Execute Count Query
            long? total = null;
            if (request.Page.HasValue && request.PageSize.HasValue)
            {
                var countQuery = query.Clone();
                //countQuery.ClearOrder();
                //countQuery.ClearLimit();
                //countQuery.ClearOffset();
                total = await _queryEngine.CountAsync(countQuery, connectionString);
            }
            #endregion

            #region Build Metadata
            List<FieldMeta> fieldsMeta;
            bool isGroupingActive = request.GroupBy != null && request.GroupBy.Any();

            if (isGroupingActive)
            {
                fieldsMeta = new List<FieldMeta>();
                if (request.GroupBy != null)
                {
                    foreach (var g in request.GroupBy)
                    {
                        var originalField = report.Fields.FirstOrDefault(f => f.Name == g);
                        fieldsMeta.Add(new FieldMeta
                        {
                            Name = g,
                            DisplayName = originalField?.DisplayName ?? g,
                            DataType = "Group",
                            IsVisible = true
                        });
                    }
                }
                if (request.Aggregates != null)
                {
                    foreach (var agg in request.Aggregates)
                    {
                        fieldsMeta.Add(new FieldMeta
                        {
                            Name = !string.IsNullOrEmpty(agg.Alias) ? agg.Alias : $"{agg.Function}_{agg.Field}",
                            DisplayName = !string.IsNullOrEmpty(agg.Alias) ? agg.Alias : $"{agg.Function} of {agg.Field}",
                            DataType = "Aggregate",
                            IsVisible = true
                        });
                    }
                }
            }
            else
            {
                fieldsMeta = (request.SelectedFields == null || !request.SelectedFields.Any())
                    ? report.Fields.OrderBy(f => f.ReportFieldId).Select(f => ToFieldMeta(f)).ToList()
                    : report.Fields.Where(f => request.SelectedFields.Contains(f.Name)).Select(f => ToFieldMeta(f)).ToList();
            }
            #endregion

            return new RunReportCommandResponse
            {
                Data = rows,
                TotalCount = total,
                Fields = fieldsMeta
            };
        }

        private Query BuildQuery(Domain.Entities.Report report, RunReportCommandRequest req)
        {
            var q = new Query().FromRaw(report.Query);

            #region Prepare Parameters
            var normalizedParams = new Dictionary<string, object?>();
            if (req.Parameters != null)
            {
                foreach (var kv in req.Parameters)
                    normalizedParams[kv.Key] = NormalizeParameterValue(kv.Value);
            }
            #endregion

            #region Determine Strategy
            var activeGroups = req.GroupBy ?? new List<string>();
            bool isGrouping = activeGroups.Any();
            #endregion

            #region Selection Logic
            if (isGrouping)
            {
                foreach (var groupField in activeGroups)
                {
                    var fieldMeta = report.Fields.FirstOrDefault(f => f.Name == groupField);
                    var expression = fieldMeta?.Expression ?? groupField;
                    q.SelectRaw($"{expression} AS [{groupField}]");
                }

                if (req.Aggregates != null)
                {
                    foreach (var agg in req.Aggregates)
                    {
                        var fieldMeta = report.Fields.FirstOrDefault(f => f.Name == agg.Field);
                        var expression = fieldMeta?.Expression ?? agg.Field;
                        var alias = string.IsNullOrEmpty(agg.Alias) ? $"{agg.Function}_{agg.Field}" : agg.Alias;
                        q.SelectRaw($"{agg.Function}({expression}) AS [{alias}]");
                    }
                }
            }
            else
            {
                var selectedFields = (req.SelectedFields == null || !req.SelectedFields.Any())
                   ? report.Fields.Where(f => f.IsVisible).ToList()
                   : report.Fields.Where(f => req.SelectedFields.Contains(f.Name)).ToList();

                foreach (var f in selectedFields)
                {
                    q.SelectRaw($"{f.Expression} AS [{f.Name}]");
                }
            }
            #endregion

            #region Filter Logic
            if (report.Filters != null && normalizedParams.Any())
            {
                foreach (var rf in report.Filters)
                {
                    if (!normalizedParams.ContainsKey(rf.ParameterName) || normalizedParams[rf.ParameterName] == null)
                        continue;
                    ApplyFilter(q, rf.FieldName, rf.Operator.ToString(), normalizedParams[rf.ParameterName]);
                }
            }

            if (req.AdHocFilters != null)
            {
                foreach (var filter in req.AdHocFilters)
                {
                    var fieldMeta = report.Fields.FirstOrDefault(f => f.Name == filter.Field);
                    var expression = fieldMeta?.Expression ?? filter.Field;
                    ApplyFilter(q, expression, filter.Operator, NormalizeParameterValue(filter.Value));
                }
            }

            #endregion

            #region Grouping Logic
            if (isGrouping)
            {
                foreach (var g in activeGroups)
                {
                    var fieldMeta = report.Fields.FirstOrDefault(f => f.Name == g);
                    var expression = fieldMeta?.Expression ?? g;
                    q.GroupByRaw(expression);
                }
            }
            #endregion

            #region Sorting Logic
            if (req.Sort != null && req.Sort.Any())
            {
                foreach (var s in req.Sort)
                {
                    var direction = s.Direction.ToUpper() == "DESC" ? "DESC" : "ASC";
                    var fieldMeta = report.Fields.FirstOrDefault(f => f.Name == s.Field);
                    var expression = fieldMeta?.Expression ?? s.Field;
                    q.OrderByRaw($"{expression} {direction}");
                }
            }
            else if (!isGrouping && report.Sortings != null && report.Sortings.Any())
            {
                var ordered = report.Sortings.OrderBy(s => s.SortOrder);
                foreach (var s in ordered)
                {
                    var dir = s.Direction == SortDirection.Descending ? "DESC" : "ASC";
                    q.OrderByRaw($"{s.Expression} {dir}");
                }
            }
            #endregion

            #region Paging Logic
            if (req.Page.HasValue && req.PageSize.HasValue)
            {
                var skip = (req.Page.Value - 1) * req.PageSize.Value;
                q.Offset(skip).Limit(req.PageSize.Value);
            }
            #endregion

            return q;
        }

        #region Helpers
        private void ApplyFilter(Query q, string field, string op, object? val)
        {
            switch (op.ToLower())
            {
                case "contains":
                case "like":
                    q.WhereRaw($"{field} LIKE ?", $"%{val}%");
                    break;
                case "equal":
                case "eq":
                case "=":
                    q.WhereRaw($"{field} = ?", val);
                    break;
                case "notequal":
                case "neq":
                case "!=":
                    q.WhereRaw($"{field} <> ?", val);
                    break;
                case "greaterthan":
                case "gt":
                case ">":
                    q.WhereRaw($"{field} > ?", val);
                    break;
                case "greaterthanorequal":
                case "gte":
                case ">=":
                    q.WhereRaw($"{field} >= ?", val);
                    break;
                case "lessthan":
                case "lt":
                case "<":
                    q.WhereRaw($"{field} < ?", val);
                    break;
                case "lessthanorequal":
                case "lte":
                case "<=":
                    q.WhereRaw($"{field} <= ?", val);
                    break;
                default:
                    q.WhereRaw($"{field} = ?", val);
                    break;
            }
        }

        private FieldMeta ToFieldMeta(ReportField f) => new FieldMeta
        {
            Name = f.Name,
            DisplayName = f.DisplayName ?? f.Name,
            DataType = f.Type.ToString(),
            Width = f.Width,
            IsVisible = f.IsVisible
        };

        private object? NormalizeParameterValue(object? value)
        {
            if (value is JsonElement json)
            {
                switch (json.ValueKind)
                {
                    case JsonValueKind.String: return json.GetString();
                    case JsonValueKind.Number:
                        if (json.TryGetInt32(out int i)) return i;
                        if (json.TryGetInt64(out long l)) return l;
                        if (json.TryGetDecimal(out decimal dec)) return dec;
                        return json.GetDouble();
                    case JsonValueKind.True:
                    case JsonValueKind.False: return json.GetBoolean();
                    case JsonValueKind.Null:
                    case JsonValueKind.Undefined: return null;
                    default: return json.ToString();
                }
            }
            return value;
        }
        #endregion
    }
}