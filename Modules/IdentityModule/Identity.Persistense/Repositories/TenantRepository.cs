using Identity.Application.Contracts.Persistence;
using Identity.Domain.Entities;
using Identity.Persistense.Context;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistense.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private readonly IdentityDbContext _context;

        public TenantRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<Tenant?> GetByCodeAsync(string code)
        {
            return await _context.Set<Tenant>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(t => t.Code == code.ToUpper());
        }

        public async Task<Tenant?> GetByIdAsync(string id)
        {
            return await _context.Set<Tenant>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tenant> CreateAsync(Tenant tenant)
        {
            await _context.Set<Tenant>().AddAsync(tenant);
            return tenant;
        }

        public async Task<bool> ExistsAsync(string code)
        {
            return await _context.Set<Tenant>()
                .IgnoreQueryFilters()
                .AnyAsync(t => t.Code == code.ToUpper());
        }
    }
}
