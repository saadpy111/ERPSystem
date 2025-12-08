using Microsoft.EntityFrameworkCore;
using Report.Domain.Entities;
using Report.Persistence.Context;

namespace Report.Persistence.Seeders
{
    public class InventoryReportSeedService
    {
        private readonly ReportDbContext _context;

        public InventoryReportSeedService(ReportDbContext context)
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
                .FirstOrDefaultAsync(r => r.Query == "Report.InventoryReports");

            if (report == null)
            {
                report = new Report.Domain.Entities.Report
                {
                    Query = "Report.InventoryReports",
                    CreatedAt = now
                };
                _context.Reports.Add(report);
            }

            report.Name = "Inventory Report";
            report.Description = "Shows full inventory status across warehouses";
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
                new() { Name = "ProductName", DisplayName = "Product Name", Expression = "ProductName", Type = Domain.Enums.FieldType.Text, Width = 200 },
                new() { Name = "Sku", DisplayName = "SKU", Expression = "Sku", Type = Domain.Enums.FieldType.Text, Width = 150 },
                new() { Name = "Barcode", DisplayName = "Barcode", Expression = "Barcode", Type = Domain.Enums.FieldType.Text, Width = 150 },
                new() { Name = "CategoryName", DisplayName = "Category", Expression = "CategoryName", Type = Domain.Enums.FieldType.Text, Width = 150 },
                new() { Name = "WarehouseName", DisplayName = "Warehouse", Expression = "WarehouseName", Type = Domain.Enums.FieldType.Text, Width = 150 },
                new() { Name = "LocationName", DisplayName = "Location", Expression = "LocationName", Type = Domain.Enums.FieldType.Text, Width = 150 },

                new() { Name = "AvailableQuantity", DisplayName = "Available Qty", Expression = "AvailableQuantity", Type = Domain.Enums.FieldType.Number, Width = 120 },
                new() { Name = "ReservedQuantity", DisplayName = "Reserved Qty", Expression = "ReservedQuantity", Type = Domain.Enums.FieldType.Number, Width = 120 },
                new() { Name = "QuarantineQuantity", DisplayName = "Quarantine Qty", Expression = "QuarantineQuantity", Type = Domain.Enums.FieldType.Number, Width = 120 },
                new() { Name = "TotalQuantity", DisplayName = "Total Qty", Expression = "TotalQuantity", Type = Domain.Enums.FieldType.Number, Width = 120 },

                new() { Name = "UnitCost", DisplayName = "Unit Cost", Expression = "UnitCost", Type = Domain.Enums.FieldType.Number, Width = 120 },
                new() { Name = "TotalCost", DisplayName = "Total Cost", Expression = "TotalCost", Type = Domain.Enums.FieldType.Number, Width = 150 },

                new() { Name = "IsLowStock", DisplayName = "Low Stock", Expression = "IsLowStock", Type = Domain.Enums.FieldType.Boolean, Width = 100 },
                new() { Name = "IsOutOfStock", DisplayName = "Out Of Stock", Expression = "IsOutOfStock", Type = Domain.Enums.FieldType.Boolean, Width = 120 },

                new() { Name = "LastStockMoveDate", DisplayName = "Last Move Date", Expression = "LastStockMoveDate", Type = Domain.Enums.FieldType.Date, Width = 150 },
                new() { Name = "LastStockMoveType", DisplayName = "Last Move Type", Expression = "LastStockMoveType", Type = Domain.Enums.FieldType.Text, Width = 150 }
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
                new() { Name = "WarehouseName", DisplayName = "Warehouse", DataType = Domain.Enums.ParameterDataType.String },
                new() { Name = "CategoryName", DisplayName = "Category", DataType = Domain.Enums.ParameterDataType.String },
                new() { Name = "MinQuantity", DisplayName = "Minimum Quantity", DataType = Domain.Enums.ParameterDataType.Decimal },
                new() { Name = "MaxQuantity", DisplayName = "Maximum Quantity", DataType = Domain.Enums.ParameterDataType.Decimal },
                new() { Name = "OnlyLowStock", DisplayName = "Only Low Stock", DataType = Domain.Enums.ParameterDataType.Boolean }
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
                new() { FieldName = "WarehouseName", Operator = Domain.Enums.FilterOperator.Equal, ParameterName = "WarehouseName" },
                new() { FieldName = "CategoryName", Operator = Domain.Enums.FilterOperator.Equal, ParameterName = "CategoryName" },
                new() { FieldName = "AvailableQuantity", Operator = Domain.Enums.FilterOperator.GreaterThan, ParameterName = "MinQuantity" },
                new() { FieldName = "AvailableQuantity", Operator = Domain.Enums.FilterOperator.LessThan, ParameterName = "MaxQuantity" },
                new() { FieldName = "IsLowStock", Operator = Domain.Enums.FilterOperator.Equal, ParameterName = "OnlyLowStock" }
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
                new() { Expression = "ProductName", Direction = Domain.Enums.SortDirection.Ascending, SortOrder = 1 },
                new() { Expression = "TotalQuantity", Direction = Domain.Enums.SortDirection.Descending, SortOrder = 2 }
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
            var groups = new List<ReportGroup>
            {
                new() { Expression = "WarehouseName", SortOrder = 1 },
                new() { Expression = "CategoryName", SortOrder = 2 }
            };

            foreach (var g in groups)
            {
                var existing = await _context.ReportGroups
                    .FirstOrDefaultAsync(x => x.ReportId == reportId && x.Expression == g.Expression);

                if (existing == null)
                {
                    g.ReportId = reportId;
                    g.CreatedAt = now;
                    g.UpdatedAt = now;
                    _context.ReportGroups.Add(g);
                }
                else
                {
                    existing.SortOrder = g.SortOrder;
                    existing.UpdatedAt = now;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
