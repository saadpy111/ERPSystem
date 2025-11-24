using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Application.Contracts.Persistence.Repositories
{
    public interface IReportsRepository
    {
        Task<Report.Domain.Entities.Report?> GetByIdAsync(int id);
        Task<IEnumerable<Report.Domain.Entities.Report>> GetAllAsync();
    }
}