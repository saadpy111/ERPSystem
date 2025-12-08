using Microsoft.EntityFrameworkCore;
using Report.Domain.Entities;
using Report.Persistence.Context;

namespace Report.Persistence.Seeders
{
    public class EmployeeReportSeedService
    {
        private readonly ReportDbContext _context;

        public EmployeeReportSeedService(ReportDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            var now = DateTime.UtcNow;

            // ---------------------------
            // DataSource (Upsert)
            // ---------------------------
            var ds = await _context.ReportDataSources
                .FirstOrDefaultAsync(x => x.Name == "SQL Server");

            if (ds == null)
            {
                ds = new ReportDataSource
                {
                    Name = "SQL Server",
                    Type = 0,
                    CreatedAt = now
                };
                _context.ReportDataSources.Add(ds);
            }

            ds.UpdatedAt = now;
            await _context.SaveChangesAsync();

            // ---------------------------
            // Report (Upsert)
            // ---------------------------
            var report = await _context.Reports
                .FirstOrDefaultAsync(r => r.Query == "Report.EmployeeReports");

            if (report == null)
            {
                report = new Report.Domain.Entities.Report
                {
                    Query = "Report.EmployeeReports",
                    CreatedAt = now
                };
                _context.Reports.Add(report);
            }

            report.Name = "Employee Report";
            report.Description = "Shows employees and job information";
            report.IsActive = true;
            report.ReportDataSourceId = ds.ReportDataSourceId;
            report.UpdatedAt = now;

            await _context.SaveChangesAsync();
            var reportId = report.ReportId;

            // ---------------------------
            // Fields (Upsert)
            // ---------------------------
            var fields = new List<ReportField>
            {
                new() { Name = "EmployeeId", DisplayName = "Employee ID", Expression = "EmployeeId", Type = Domain.Enums.FieldType.Text, Width = 100 },
                new() { Name = "FullName", DisplayName = "Full Name", Expression = "FullName", Type = 0, Width = 200 },
                new() { Name = "DepartmentName", DisplayName = "Department", Expression = "DepartmentName", Type = 0, Width = 200 },
                new() { Name = "JobTitle", DisplayName = "Job Title", Expression = "JobTitle", Type = 0, Width = 200 },
                new() { Name = "ManagerName", DisplayName = "Manager", Expression = "ManagerName", Type = 0, Width = 200 },
                new() { Name = "HireDate", DisplayName = "Hire Date", Expression = "HireDate", Type = Domain.Enums.FieldType.Date, Width = 150 },
                new() { Name = "Salary", DisplayName = "Salary", Expression = "Salary", Type = Domain.Enums.FieldType.Number, Width = 150 },
                new() { Name = "ContractType", DisplayName = "Contract Type", Expression = "ContractType", Type = 0, Width = 150 }
            };

            foreach (var field in fields)
            {
                var existing = await _context.ReportFields
                    .FirstOrDefaultAsync(f => f.ReportId == reportId && f.Name == field.Name);

                if (existing == null)
                {
                    field.ReportId = reportId;
                    field.CreatedAt = now;
                    field.UpdatedAt = now;
                    field.IsVisible = true;
                    _context.ReportFields.Add(field);
                }
                else
                {
                    existing.DisplayName = field.DisplayName;
                    existing.Expression = field.Expression;
                    existing.Type = field.Type;
                    existing.Width = field.Width;
                    existing.UpdatedAt = now;
                }
            }

            // ---------------------------
            // Parameters (Upsert)
            // ---------------------------
            var parameters = new List<ReportParameter>
            {
                new() { Name = "DepartmentName", DisplayName = "Department", DataType =  Domain.Enums.ParameterDataType.String },
                new() { Name = "MinSalary", DisplayName = "Minimum Salary", DataType =  Domain.Enums.ParameterDataType.Decimal },
                new() { Name = "MaxSalary", DisplayName = "Maximum Salary", DataType = Domain.Enums.ParameterDataType.Decimal },
                new() { Name = "SearchName", DisplayName = "Name Contains", DataType =  Domain.Enums.ParameterDataType.String },
                new() { Name = "HireDateFrom", DisplayName = "Hire Date From", DataType =  Domain.Enums.ParameterDataType.DateTime },
                new() { Name = "HireDateTo", DisplayName = "Hire Date To", DataType = Domain.Enums.ParameterDataType.DateTime }
            };

            foreach (var p in parameters)
            {
                var existing = await _context.ReportParameters
                    .FirstOrDefaultAsync(x => x.ReportId == reportId && x.Name == p.Name);

                if (existing == null)
                {
                    p.ReportId = reportId;
                    p.CreatedAt = now;
                    p.UpdatedAt = now;
                    p.IsRequired = false;
                    _context.ReportParameters.Add(p);
                }
                else
                {
                    existing.DisplayName = p.DisplayName;
                    existing.DataType = p.DataType;
                    existing.IsRequired = false;
                    existing.UpdatedAt = now;
                }
            }

            // ---------------------------
            // Filters (Upsert)
            // ---------------------------
            var filters = new List<ReportFilter>
            {
                new() { FieldName = "DepartmentName", Operator = Domain.Enums.FilterOperator.Equal, ParameterName = "DepartmentName" },
                new() { FieldName = "Salary", Operator = Domain.Enums.FilterOperator.GreaterThan, ParameterName = "MinSalary" },
                new() { FieldName = "Salary", Operator = Domain.Enums.FilterOperator.LessThan, ParameterName = "MaxSalary" },
                new() { FieldName = "FullName", Operator = Domain.Enums.FilterOperator.Contains, ParameterName = "SearchName" },
                new() { FieldName = "HireDate", Operator = Domain.Enums.FilterOperator.GreaterThan, ParameterName = "HireDateFrom" },
                new() { FieldName = "HireDate", Operator = Domain.Enums.FilterOperator.LessThan, ParameterName = "HireDateTo" }
            };

            foreach (var f in filters)
            {
                var existing = await _context.ReportFilters.FirstOrDefaultAsync(x =>
                    x.ReportId == reportId &&
                    x.FieldName == f.FieldName &&
                    x.ParameterName == f.ParameterName);

                if (existing == null)
                {
                    f.ReportId = reportId;
                    f.CreatedAt = now;
                    f.UpdatedAt = now;
                    _context.ReportFilters.Add(f);
                }
                else
                {
                    existing.Operator = f.Operator;
                    existing.UpdatedAt = now;
                }
            }

            // ---------------------------
            // Sorting (Upsert)
            // ---------------------------
            var sortings = new List<ReportSorting>
            {
                new() { Expression = "FullName", Direction = Domain.Enums.SortDirection.Ascending, SortOrder = 1 },
                new() { Expression = "Salary", Direction = Domain.Enums.SortDirection.Descending, SortOrder = 2 }
            };

            foreach (var s in sortings)
            {
                var existing = await _context.ReportSortings
                    .FirstOrDefaultAsync(x => x.ReportId == reportId && x.Expression == s.Expression);

                if (existing == null)
                {
                    s.ReportId = reportId;
                    s.CreatedAt = now;
                    s.UpdatedAt = now;
                    _context.ReportSortings.Add(s);
                }
                else
                {
                    existing.Direction = s.Direction;
                    existing.SortOrder = s.SortOrder;
                    existing.UpdatedAt = now;
                }
            }

            // ---------------------------
            // Grouping (Optional)
            // ---------------------------
            var group = await _context.ReportGroups
                .FirstOrDefaultAsync(g => g.ReportId == reportId && g.Expression == "DepartmentName");

            if (group == null)
            {
                group = new ReportGroup
                {
                    ReportId = reportId,
                    Expression = "DepartmentName",
                    SortOrder = 1,
                    CreatedAt = now,
                    UpdatedAt = now
                };
                _context.ReportGroups.Add(group);
            }

            await _context.SaveChangesAsync();
        }
    }
}
