using Accounting.Application.Reports.Builders;
using Accounting.Application.Reports.Models;
using Accounting.Application.Reports.Models.Columns;
using Accounting.Application.Reports.Services;
using ClosedXML.Excel;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Persistence.External.Services.Generators
{
    public class ExcelReportGenerator
    {
        private readonly IFinancialFormattingService _formatter;

        public ExcelReportGenerator(IFinancialFormattingService formatter)
        {
            _formatter = formatter;
        }

        public async Task<(Stream Stream, string ContentType, string FileName)> Generate<T>(ReportResponse<T> response, ReportDefinition<T> definition)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(response.Metadata.ReportName.Substring(0, Math.Min(response.Metadata.ReportName.Length, 31))); // Max length for sheet name is 31
            var visibleColumns = definition.Columns.Where(c => c.IsVisible).ToList();

            // Meta Info
            worksheet.Cell(1, 1).Value = response.Metadata.CompanyName;
            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Cell(1, 1).Style.Font.FontSize = 14;

            worksheet.Cell(2, 1).Value = response.Metadata.ReportName;
            worksheet.Cell(2, 1).Style.Font.Bold = true;

            int row = 4;

            // Header
            for (int i = 0; i < visibleColumns.Count; i++)
            {
                var cell = worksheet.Cell(row, i + 1);
                cell.Value = visibleColumns[i].HeaderText;
                cell.Style.Font.Bold = true;
                cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            worksheet.SheetView.FreezeRows(row);
            row++;

            // Data
            foreach (var item in response.Items)
            {
                for (int i = 0; i < visibleColumns.Count; i++)
                {
                    var col = visibleColumns[i];
                    var val = col.ValueSelector(item);
                    var cell = worksheet.Cell(row, i + 1);

                    if (val is decimal decVal)
                    {
                        if (col is DebitColumn<T> debitCol)
                        {
                            var balance = _formatter.GetDebitBalance(decVal, debitCol.AccountTypeSelector(item));
                            cell.Value = balance;
                            cell.Style.NumberFormat.Format = "#,##0.00";
                        }
                        else if (col is CreditColumn<T> creditCol)
                        {
                            var balance = _formatter.GetCreditBalance(decVal, creditCol.AccountTypeSelector(item));
                            cell.Value = balance;
                            cell.Style.NumberFormat.Format = "#,##0.00";
                        }
                        else if (col is CurrencyColumn<T> currCol)
                        {
                            cell.Value = decVal;
                            cell.Style.NumberFormat.Format = $"#,##0.00 \"{currCol.CurrencyCodeSelector(item) ?? response.Metadata.CurrencyCode ?? ""}\"";
                        }
                        else if (col is PercentageColumn<T>)
                        {
                            cell.Value = decVal / 100m;
                            cell.Style.NumberFormat.Format = "0.00%";
                        }
                        else if (col is DecimalColumn<T>)
                        {
                            cell.Value = decVal;
                            cell.Style.NumberFormat.Format = "#,##0.00";
                        }
                        else
                        {
                            cell.Value = decVal;
                        }
                    }
                    else
                    {
                        cell.Value = val?.ToString() ?? string.Empty;
                        
                        if (col is HierarchicalLabelColumn<T> hlCol)
                        {
                            var indent = hlCol.IndentLevelSelector(item);
                            if (indent > 0)
                            {
                                cell.Style.Alignment.Indent = indent;
                            }
                        }
                    }
                }
                row++;
            }

            // Totals
            if (definition.IncludeTotals && response.Totals != null && response.Totals.Any())
            {
                var totalRow = row + 1;
                worksheet.Cell(totalRow, 1).Value = "Totals";
                worksheet.Cell(totalRow, 1).Style.Font.Bold = true;

                for (int i = 0; i < visibleColumns.Count; i++)
                {
                    var col = visibleColumns[i];
                    var cell = worksheet.Cell(totalRow, i + 1);

                    if (response.Totals.TryGetValue(col.HeaderText, out decimal totalValue))
                    {
                        cell.Value = totalValue;
                        cell.Style.NumberFormat.Format = "#,##0.00";
                        cell.Style.Font.Bold = true;
                        cell.Style.Border.TopBorder = XLBorderStyleValues.Double;
                    }
                }
            }

            worksheet.Columns().AdjustToContents();

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"{response.Metadata.ReportName.Replace(" ", "")}_{DateTime.UtcNow:yyyy-MM-dd}.xlsx";
            return await Task.FromResult((stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName));
        }
    }
}
