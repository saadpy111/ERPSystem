using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using Accounting.Persistence.Common;
using Accounting.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Persistence.Repositories.Implementations
{
    public class AccountingMappingRepository : GenericRepository<AccountingMapping>, IAccountingMappingRepository
    {
        public AccountingMappingRepository(AccountingDbContext context) : base(context)
        {
        }

        public async Task<AccountingMapping?> GetByKeyAsync(SourceType sourceType, string key)
        {
            return await _dbSet.FirstOrDefaultAsync(m => m.SourceType == sourceType && m.MappingKey == key);
        }

        public async Task<List<AccountingMapping>> GetBySourceAsync(SourceType sourceType)
        {
            return await _dbSet.Where(m => m.SourceType == sourceType).ToListAsync();
        }
    }
}
