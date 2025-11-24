using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Application.Contracts.Persistence.Repositories
{
    public interface IReportSortingsRepository
    {
        Task<Report.Domain.Entities.ReportSorting?> GetByIdAsync(int id);
        Task<IEnumerable<Report.Domain.Entities.ReportSorting>> GetAllAsync();
    }
}