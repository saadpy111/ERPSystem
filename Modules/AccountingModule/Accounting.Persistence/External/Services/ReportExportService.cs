using Accounting.Application.Interfaces.Services;
using ClosedXML.Excel;
using CsvHelper;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Accounting.Persistence.External.Services
{
    public class ReportExportService : IReportExportService
    {
        public ReportExportService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<(Stream Stream, string ContentType, string FileName)> ExportToPdf<T>(T reportData, string reportName)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(c => ComposeHeader(c, reportName, reportData));
                    page.Content().Element(c => ComposeContent(c, reportData));
                    page.Footer().Element(ComposeFooter);
                });
            });

            var stream = new MemoryStream();
            document.GeneratePdf(stream);
            stream.Position = 0;

            var fileName = $"{reportName.Replace(" ", "")}_{DateTime.UtcNow:yyyy-MM-dd}.pdf";
            return await Task.FromResult((stream, "application/pdf", fileName));
        }

        public async Task<(Stream Stream, string ContentType, string FileName)> ExportToExcel<T>(IEnumerable<T> data, string reportName)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(reportName);

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            for (int i = 0; i < properties.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = properties[i].Name;
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            }

            var row = 2;
            foreach (var item in data)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    var val = properties[i].GetValue(item);
                    worksheet.Cell(row, i + 1).Value = val?.ToString() ?? string.Empty;
                }
                row++;
            }

            worksheet.Columns().AdjustToContents();

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"{reportName.Replace(" ", "")}_{DateTime.UtcNow:yyyy-MM-dd}.xlsx";
            return await Task.FromResult((stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName));
        }

        public async Task<(Stream Stream, string ContentType, string FileName)> ExportToCsv<T>(IEnumerable<T> data, string reportName)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteRecords(data);
            await writer.FlushAsync();
            stream.Position = 0;

            var fileName = $"{reportName.Replace(" ", "")}_{DateTime.UtcNow:yyyy-MM-dd}.csv";
            return (stream, "text/csv", fileName);
        }

        private void ComposeHeader<T>(IContainer container, string reportName, T reportData)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("ERP System").FontSize(20).SemiBold().FontColor(Colors.Blue.Darken2);
                    column.Item().Text(reportName).FontSize(14);
                    
                    var dateProp = typeof(T).GetProperty("FromDate");
                    if (dateProp != null)
                    {
                        var fromDate = (DateTime?)dateProp.GetValue(reportData);
                        var toDateProp = typeof(T).GetProperty("ToDate");
                        var toDate = (DateTime?)toDateProp?.GetValue(reportData);
                        
                        if (fromDate.HasValue && toDate.HasValue)
                        {
                            column.Item().Text($"Date Range: {fromDate.Value:yyyy-MM-dd} to {toDate.Value:yyyy-MM-dd}");
                        }
                        else if (fromDate.HasValue)
                        {
                            column.Item().Text($"As of Date: {fromDate.Value:yyyy-MM-dd}");
                        }
                    }
                });
                
                row.ConstantItem(100).AlignRight().Text($"Date: {DateTime.UtcNow:yyyy-MM-dd}");
            });
        }

        private void ComposeContent<T>(IContainer container, T reportData)
        {
            container.PaddingVertical(1, Unit.Centimetre).Column(column =>
            {
                // Find IEnumerable property for the table
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var listProperty = properties.FirstOrDefault(p => typeof(IEnumerable).IsAssignableFrom(p.PropertyType) && p.PropertyType != typeof(string));
                
                if (listProperty != null)
                {
                    var listData = (IEnumerable)listProperty.GetValue(reportData);
                    if (listData != null)
                    {
                        var itemType = listProperty.PropertyType.IsGenericType ? listProperty.PropertyType.GetGenericArguments()[0] : null;
                        
                        if (itemType != null)
                        {
                            var itemProperties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                            
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    foreach (var _ in itemProperties)
                                    {
                                        columns.RelativeColumn();
                                    }
                                });

                                table.Header(header =>
                                {
                                    foreach (var prop in itemProperties)
                                    {
                                        header.Cell().BorderBottom(1).BorderColor(Colors.Black).PaddingBottom(5).Text(prop.Name).SemiBold();
                                    }
                                });

                                foreach (var item in listData)
                                {
                                    foreach (var prop in itemProperties)
                                    {
                                        var val = prop.GetValue(item);
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5).Text(val?.ToString() ?? string.Empty);
                                    }
                                }
                            });
                        }
                    }
                }

                // Add totals (any scalar numeric properties not named "Id")
                column.Item().PaddingTop(25).Column(totalsColumn => 
                {
                    var numericProps = properties.Where(p => 
                        (p.PropertyType == typeof(decimal) || p.PropertyType == typeof(double) || p.PropertyType == typeof(int)) &&
                        !p.Name.EndsWith("Id") && !p.Name.Equals("Year") && !p.Name.Equals("Month")
                    ).ToList();

                    if (numericProps.Any())
                    {
                        totalsColumn.Item().PaddingBottom(5).Text("Summary Totals").SemiBold().FontSize(12);
                        
                        foreach(var prop in numericProps)
                        {
                            var val = prop.GetValue(reportData);
                            totalsColumn.Item().Row(r => 
                            {
                                r.RelativeItem().Text(prop.Name);
                                r.RelativeItem().AlignRight().Text(val?.ToString() ?? "0").SemiBold();
                            });
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
