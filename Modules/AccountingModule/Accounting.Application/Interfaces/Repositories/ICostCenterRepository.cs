using Accounting.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounting.Application.Interfaces.Repositories
{
    public interface ICostCenterRepository : IGenericRepository<CostCenter>
    {
        Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null);
        Task<bool> HasTransactionsAsync(int id);
        Task<List<CostCenter>> GetHierarchyAsync();
        Task<bool> IsDescendantAsync(int parentId, int childId);
    }
}
