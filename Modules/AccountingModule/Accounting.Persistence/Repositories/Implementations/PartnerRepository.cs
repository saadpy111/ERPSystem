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
    public class PartnerRepository : GenericRepository<Partner>, IPartnerRepository
    {
        public PartnerRepository(AccountingDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Partner>> GetCustomersAsync()
        {
            return await _dbSet
                .Where(p => p.Type == PartnerType.Customer)
                .ToListAsync();
        }

        public async Task<IEnumerable<Partner>> GetVendorsAsync()
        {
            return await _dbSet
                .Where(p => p.Type == PartnerType.Vendor)
                .ToListAsync();
        }
    }
}
