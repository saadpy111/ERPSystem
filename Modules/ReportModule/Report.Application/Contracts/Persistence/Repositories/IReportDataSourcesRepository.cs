using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Application.Contracts.Persistence.Repositories
{
    public interface IReportDataSourcesRepository
    {
        Task<Report.Domain.Entities.ReportDataSource?> GetByIdAsync(int id);
        Task<IEnumerable<Report.Domain.Entities.ReportDataSource>> GetAllAsync();
    }
}