using MediatR;
using System.Text.Json;

namespace Report.Application.Features.ReportFeatures.RunReport
{
    public class RunReportCommandRequest : IRequest<RunReportCommandResponse>
    {
        public int ReportId { get; set; }
        public List<string>? SelectedFields { get; set; }
        public Dictionary<string, object?>? Parameters { get; set; }
        public List<FilterRequest>? AdHocFilters { get; set; }
        public List<string>? GroupBy { get; set; }
        public List<AggregateRequest>? Aggregates { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public List<SortRequest>? Sort { get; set; }
    }

    public class SortRequest
    {
        public string Field { get; set; } = string.Empty;
        public string Direction { get; set; } = "ASC";
    }

    public class FilterRequest
    {
        public string Field { get; set; } = string.Empty;
        public string Operator { get; set; } = "eq";
        public object? Value { get; set; }
    }

    public class AggregateRequest
    {
        public string Field { get; set; } = string.Empty;
        public string Function { get; set; } = "SUM";
        public string Alias { get; set; } = string.Empty;
    }
}