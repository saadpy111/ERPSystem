using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Application.Contracts.Persistence.Repositories
{
    public interface IBaseReportRepository
    {
        Task<T?> GetByIdAsync<T>(int id) where T : class;
        Task<IEnumerable<T>> GetAllAsync<T>() where T : class;
    }
}