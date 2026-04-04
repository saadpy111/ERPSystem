using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounting.Application.Interfaces.Repositories
{
    public interface IAccountingMappingRepository : IGenericRepository<AccountingMapping>
    {
        Task<AccountingMapping?> GetByKeyAsync(SourceType sourceType, string key);
        Task<List<AccountingMapping>> GetBySourceAsync(SourceType sourceType);
    }
}
