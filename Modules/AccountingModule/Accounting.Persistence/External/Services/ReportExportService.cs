using Accounting.Application.Interfaces.Services;
using Accounting.Application.Reports.Builders;
using Accounting.Application.Reports.Models;
using Accounting.Persistence.External.Services.Generators;
using QuestPDF.Infrastructure;
using System.IO;
using System.Threading.Tasks;

namespace Accounting.Persistence.External.Services
{
    public class ReportExportService : IReportExportService
    {
        private readonly PdfReportGenerator _pdfGenerator;
        private readonly ExcelReportGenerator _excelGenerator;
        private readonly CsvReportGenerator _csvGenerator;

        public ReportExportService(
            Accounting.Application.Reports.Services.IFinancialFormattingService formatter)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            _pdfGenerator = new PdfReportGenerator(formatter);
            _excelGenerator = new ExcelReportGenerator(formatter);
            _csvGenerator = new CsvReportGenerator(formatter);
        }

        public async Task<(Stream Stream, string ContentType, string FileName)> ExportToPdf<T>(ReportResponse<T> response, ReportDefinition<T> definition)
        {
            return await _pdfGenerator.Generate(response, definition);
        }

        public async Task<(Stream Stream, string ContentType, string FileName)> ExportToExcel<T>(ReportResponse<T> response, ReportDefinition<T> definition)
        {
            return await _excelGenerator.Generate(response, definition);
        }

        public async Task<(Stream Stream, string ContentType, string FileName)> ExportToCsv<T>(ReportResponse<T> response, ReportDefinition<T> definition)
        {
            return await _csvGenerator.Generate(response, definition);
        }
    }
}
