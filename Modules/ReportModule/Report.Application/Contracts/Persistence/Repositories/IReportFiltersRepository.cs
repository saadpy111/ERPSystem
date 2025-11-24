using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Application.Contracts.Persistence.Repositories
{
    public interface IReportFiltersRepository
    {
        Task<Report.Domain.Entities.ReportFilter?> GetByIdAsync(int id);
        Task<IEnumerable<Report.Domain.Entities.ReportFilter>> GetAllAsync();
    }
}