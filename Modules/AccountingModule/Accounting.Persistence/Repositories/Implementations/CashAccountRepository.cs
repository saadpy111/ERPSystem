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
    public class CashAccountRepository : GenericRepository<CashAccount>, ICashAccountRepository
    {
        public CashAccountRepository(AccountingDbContext context) : base(context)
        {
        }

        public Task<CashAccount?> GetWithAccountAsync(int id)
        {
            return _dbSet
                .Include(c => c.Account)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<List<CashAccount>> GetAllWithAccountAsync()
        {
            return _dbSet
                .AsNoTracking()
                .Include(c => c.Account)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
    }
}
