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
    public class PayableRepository : GenericRepository<Payable>, IPayableRepository
    {
        public PayableRepository(AccountingDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Payable>> GetByPartnerAsync(int partnerId)
        {
            return await _dbSet.Where(p => p.PartnerId == partnerId).ToListAsync();
        }

        public async Task<IEnumerable<Payable>> GetOpenItemsAsync()
        {
            return await _dbSet.Where(p => p.Status == PayableStatus.Open || p.Status == PayableStatus.PartiallyPaid).ToListAsync();
        }
    }
}
