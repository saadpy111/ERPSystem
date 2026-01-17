using Identity.Domain.Entities;
using Identity.Persistense.Context;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistense.Seeders
{
    public class TenantSeeder
    {
        private readonly IdentityDbContext _context;

        public TenantSeeder(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Check if any tenants exist (bypass query filters)
            if (await _context.Tenants.IgnoreQueryFilters().AnyAsync())
            {
                return; // Already seeded
            }

            var tenants = new List<Tenant>
            {
                new Tenant
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Default Tenant",
                    Code = "DEFAULT",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Tenant
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Demo Company",
                    Code = "DEMO",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await _context.Tenants.AddRangeAsync(tenants);
            await _context.SaveChangesAsync();
        }
    }
}
