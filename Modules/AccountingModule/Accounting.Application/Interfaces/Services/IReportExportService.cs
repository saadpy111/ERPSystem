using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Accounting.Application.Interfaces.Services
{
    public interface IReportExportService
    {
        Task<(Stream Stream, string ContentType, string FileName)> ExportToPdf<T>(T reportData, string reportName);
        Task<(Stream Stream, string ContentType, string FileName)> ExportToExcel<T>(IEnumerable<T> data, string reportName);
        Task<(Stream Stream, string ContentType, string FileName)> ExportToCsv<T>(IEnumerable<T> data, string reportName);
    }
}
