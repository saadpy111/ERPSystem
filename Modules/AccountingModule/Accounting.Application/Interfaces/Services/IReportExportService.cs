using Accounting.Application.Reports.Builders;
using Accounting.Application.Reports.Models;
using System.IO;
using System.Threading.Tasks;

namespace Accounting.Application.Interfaces.Services
{
    public interface IReportExportService
    {
        Task<(Stream Stream, string ContentType, string FileName)> ExportToPdf<T>(ReportResponse<T> response, ReportDefinition<T> definition);
        Task<(Stream Stream, string ContentType, string FileName)> ExportToExcel<T>(ReportResponse<T> response, ReportDefinition<T> definition);
        Task<(Stream Stream, string ContentType, string FileName)> ExportToCsv<T>(ReportResponse<T> response, ReportDefinition<T> definition);
    }
}
