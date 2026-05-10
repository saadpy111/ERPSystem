using Accounting.Application.Reports.Builders;
using Accounting.Application.Reports.Models;
using Accounting.Application.Reports.Models.Columns;
using Accounting.Application.Reports.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Persistence.External.Services.Generators
{
    public class PdfReportGenerator
    {
        private readonly IFinancialFormattingService _formatter;

        public PdfReportGenerator(IFinancialFormattingService formatter)
        {
            _formatter = formatter;
        }

        public async Task<(Stream Stream, string ContentType, string FileName)> Generate<T>(ReportResponse<T> response, ReportDefinition<T> definition)
        {
            var isArabic = _formatter.IsArabicCulture();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily(isArabic ? "Arial" : "Helvetica"));

                    if (isArabic)
                    {
                        page.ContentFromRightToLeft();
                    }

                    page.Header().Element(c => ComposeHeader(c, response.Metadata));
                    page.Content().Element(c => ComposeContent(c, response, definition));
                    page.Footer().Element(ComposeFooter);
                });
            });

            var stream = new MemoryStream();
            document.GeneratePdf(stream);
            stream.Position = 0;

            var fileName = $"{response.Metadata.ReportName.Replace(" ", "")}_{DateTime.UtcNow:yyyy-MM-dd}.pdf";
            return await Task.FromResult((stream, "application/pdf", fileName));
        }

        private void ComposeHeader(IContainer container, ReportMetadata metadata)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text(metadata.CompanyName).FontSize(20).SemiBold().FontColor(Colors.Blue.Darken2);
                    column.Item().Text(metadata.ReportName).FontSize(14).SemiBold();
                    
                    if (metadata.FromDate.HasValue && metadata.ToDate.HasValue)
                    {
                        column.Item().Text($"Date Range: {metadata.FromDate.Value:yyyy-MM-dd} to {metadata.ToDate.Value:yyyy-MM-dd}");
                    }
                    else if (metadata.AsOfDate.HasValue)
                    {
                        column.Item().Text($"As of Date: {metadata.AsOfDate.Value:yyyy-MM-dd}");
                    }
                });
                
                row.ConstantItem(100).AlignRight().Text($"Date: {metadata.GeneratedAt:yyyy-MM-dd}");
            });
        }

        private void ComposeContent<T>(IContainer container, ReportResponse<T> response, ReportDefinition<T> definition)
        {
            container.PaddingVertical(1, Unit.Centimetre).Column(column =>
            {
                var visibleColumns = definition.Columns.Where(c => c.IsVisible).ToList();

                if (!response.Items.Any())
                {
                    column.Item().AlignCenter().PaddingTop(50).Text("No data available for the selected period.").FontSize(12).Italic().FontColor(Colors.Grey.Medium);
                    return;
                }

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        foreach (var col in visibleColumns)
                        {
                            if (col is DebitColumn<T> || col is CreditColumn<T> || col is CurrencyColumn<T> || col is DecimalColumn<T>)
                                columns.RelativeColumn(1.2f); // Give financial columns a bit more space
                            else if (col is TextColumn<T> || col is HierarchicalLabelColumn<T>)
                                columns.RelativeColumn(2); // Give text more space
                            else
                                columns.RelativeColumn();
                        }
                    });

                    // Headers
                    table.Header(header =>
                    {
                        foreach (var col in visibleColumns)
                        {
                            var cell = header.Cell().BorderBottom(1).BorderColor(Colors.Black).PaddingBottom(5);
                            
                            var isNumeric = col is DebitColumn<T> || col is CreditColumn<T> || col is CurrencyColumn<T> || col is DecimalColumn<T> || col is PercentageColumn<T>;
                            
                            if (isNumeric)
                                cell.AlignRight().Text(col.HeaderText).SemiBold();
                            else
                                cell.AlignLeft().Text(col.HeaderText).SemiBold();
                        }
                    });

                    // Data
                    foreach (var item in response.Items)
                    {
                        foreach (var col in visibleColumns)
                        {
                            var val = col.ValueSelector(item);
                            var cell = table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(3);

                            var isNumeric = col is DebitColumn<T> || col is CreditColumn<T> || col is CurrencyColumn<T> || col is DecimalColumn<T> || col is PercentageColumn<T>;

                            if (isNumeric)
                                cell = cell.AlignRight();
                            else
                                cell = cell.AlignLeft();

                            if (col is HierarchicalLabelColumn<T> hlCol && val != null)
                            {
                                var indent = hlCol.IndentLevelSelector(item);
                                cell.PaddingLeft(indent * 10).Text(val.ToString());
                            }
                            else if (val is decimal decVal)
                            {
                                if (col is DebitColumn<T> debitCol)
                                {
                                    var balance = _formatter.GetDebitBalance(decVal, debitCol.AccountTypeSelector(item));
                                    cell.Text(_formatter.FormatDecimal(balance));
                                }
                                else if (col is CreditColumn<T> creditCol)
                                {
                                    var balance = _formatter.GetCreditBalance(decVal, creditCol.AccountTypeSelector(item));
                                    cell.Text(_formatter.FormatDecimal(balance));
                                }
                                else if (col is CurrencyColumn<T> currCol)
                                {
                                    var currCode = currCol.CurrencyCodeSelector(item) ?? response.Metadata.CurrencyCode;
                                    cell.Text(_formatter.FormatCurrency(decVal, currCode));
                                }
                                else if (col is PercentageColumn<T>)
                                {
                                    cell.Text(_formatter.FormatPercentage(decVal));
                                }
                                else if (col is DecimalColumn<T> dCol)
                                {
                                    cell.Text(decVal.ToString(dCol.Format));
                                }
                                else
                                {
                                    cell.Text(decVal.ToString());
                                }
                            }
                            else
                            {
                                cell.Text(val?.ToString() ?? string.Empty);
                            }
                        }
                    }

                    // Totals
                    if (definition.IncludeTotals && response.Totals != null && response.Totals.Any())
                    {
                        var firstCol = true;
                        foreach (var col in visibleColumns)
                        {
                            var cell = table.Cell().BorderTop(1).BorderColor(Colors.Black).PaddingTop(5).PaddingBottom(5);
                            
                            if (firstCol)
                            {
                                cell.AlignLeft().Text("Totals").SemiBold();
                                firstCol = false;
                            }
                            else if (response.Totals.TryGetValue(col.HeaderText, out decimal totalValue))
                            {
                                cell.AlignRight().Text(_formatter.FormatDecimal(totalValue)).SemiBold();
                            }
                            else
                            {
                                cell.Text("");
                            }
                        }
                    }
                });
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(x =>
            {
                x.Span("Page ");
                x.CurrentPageNumber();
                x.Span(" of ");
                x.TotalPages();
            });
        }
    }
}
