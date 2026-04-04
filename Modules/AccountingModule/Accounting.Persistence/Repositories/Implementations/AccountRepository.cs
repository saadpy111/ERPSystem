using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Persistence.Common;
using Accounting.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Persistence.Repositories.Implementations
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(AccountingDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Account>> GetAccountTreeAsync()
        {
            // Usually returns top-level parents directly or includes children
            return await _dbSet
                .Where(a => a.ParentAccountId == null)
                .Include(a => a.ChildAccounts)
                .ToListAsync();
        }

        public async Task<IEnumerable<Account>> GetLeafAccountsAsync()
        {
            // Return accounts that have no children
            return await _dbSet
                .Where(a => !_dbSet.Any(child => child.ParentAccountId == a.Id))
                .ToListAsync();
        }
    }
}
