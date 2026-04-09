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
    public class ReceivableRepository : GenericRepository<Receivable>, IReceivableRepository
    {
        public ReceivableRepository(AccountingDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Receivable>> GetByPartnerAsync(int partnerId)
        {
            return await _dbSet.Where(r => r.PartnerId == partnerId).ToListAsync();
        }

        public async Task<IEnumerable<Receivable>> GetOpenItemsAsync()
        {
            return await _dbSet.Where(r => r.Status == ReceivableStatus.Open || r.Status == ReceivableStatus.PartiallyPaid).ToListAsync();
        }
    }
}
