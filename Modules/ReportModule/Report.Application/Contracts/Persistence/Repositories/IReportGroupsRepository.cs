using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Application.Contracts.Persistence.Repositories
{
    public interface IReportGroupsRepository
    {
        Task<Report.Domain.Entities.ReportGroup?> GetByIdAsync(int id);
        Task<IEnumerable<Report.Domain.Entities.ReportGroup>> GetAllAsync();
    }
}