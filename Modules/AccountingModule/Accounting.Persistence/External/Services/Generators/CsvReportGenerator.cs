using Accounting.Application.Reports.Builders;
using Accounting.Application.Reports.Models;
using Accounting.Application.Reports.Models.Columns;
using Accounting.Application.Reports.Services;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Persistence.External.Services.Generators
{
    public class CsvReportGenerator
    {
        private readonly IFinancialFormattingService _formatter;

        public CsvReportGenerator(IFinancialFormattingService formatter)
        {
            _formatter = formatter;
        }

        public async Task<(Stream Stream, string ContentType, string FileName)> Generate<T>(ReportResponse<T> response, ReportDefinition<T> definition)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, leaveOpen: true);
            var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            // Write Headers
            var visibleColumns = definition.Columns.Where(c => c.IsVisible).ToList();
            foreach (var col in visibleColumns)
            {
                csv.WriteField(col.HeaderText);
            }
            await csv.NextRecordAsync();

            // Write Data
            foreach (var item in response.Items)
            {
                foreach (var col in visibleColumns)
                {
                    var val = col.ValueSelector(item);
                    var strVal = FormatValue(col, val, item);
                    csv.WriteField(strVal);
                }
                await csv.NextRecordAsync();
            }

            // Write Totals if any
            if (definition.IncludeTotals && response.Totals != null && response.Totals.Any())
            {
                await csv.NextRecordAsync();
                csv.WriteField("Totals");
                for (int i = 1; i < visibleColumns.Count; i++)
                {
                    var col = visibleColumns[i];
                    if (response.Totals.TryGetValue(col.HeaderText, out decimal totalValue))
                    {
                        csv.WriteField(totalValue.ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        csv.WriteField("");
                    }
                }
                await csv.NextRecordAsync();
            }

            await writer.FlushAsync();
            stream.Position = 0;

            var fileName = $"{response.Metadata.ReportName.Replace(" ", "")}_{DateTime.UtcNow:yyyy-MM-dd}.csv";
            return (stream, "text/csv", fileName);
        }

        private string FormatValue<T>(ReportColumn<T> col, object? val, T item)
        {
            if (val == null) return string.Empty;

            if (val is decimal decVal)
            {
                // We keep pure numbers for CSV where possible, but formatting applies to semantic columns
                if (col is DebitColumn<T> debitCol)
                {
                    var balance = _formatter.GetDebitBalance(decVal, debitCol.AccountTypeSelector(item));
                    return balance.ToString(CultureInfo.InvariantCulture);
                }
                else if (col is CreditColumn<T> creditCol)
                {
                    var balance = _formatter.GetCreditBalance(decVal, creditCol.AccountTypeSelector(item));
                    return balance.ToString(CultureInfo.InvariantCulture);
                }
                return decVal.ToString(CultureInfo.InvariantCulture);
            }

            return val.ToString() ?? string.Empty;
        }
    }
}
