using Identity.Domain.Entities;
using Identity.Persistense.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistense.Seeders
{
    public class UserSeeder
    {
        private readonly IdentityDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSeeder(IdentityDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            var tenants = await _context.Tenants.IgnoreQueryFilters().ToListAsync();

            foreach (var tenant in tenants)
            {
                var email = $"admin@{tenant.Code.ToLower()}.com";

                // Check if user already exists (bypass query filters since there's no tenant context during seeding)
                var existingUser = await _context.Users.IgnoreQueryFilters()
                    .FirstOrDefaultAsync(u => u.Email == email);
                    
                if (existingUser != null)
                {
                    continue; // User already exists, skip
                }

                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = email,
                    NormalizedUserName = email.ToUpper(),
                    Email = email,
                    NormalizedEmail = email.ToUpper(),
                    EmailConfirmed = true,
                    TenantId = tenant.Id,
                    FullName = "Super Administrator"
                };

                // Create user with password
                var result = await _userManager.CreateAsync(user, "SuperAdmin@123");

                if (result.Succeeded)
                {
                    // Assign SuperAdmin role (bypass query filters, use tenant-specific name)
                    var superAdminRole = await _context.Roles.IgnoreQueryFilters()
                        .FirstOrDefaultAsync(r => r.Name == $"SuperAdmin_{tenant.Code}" && r.TenantId == tenant.Id);

                    if (superAdminRole != null)
                    {
                        var userRole = new ApplicationUserRole
                        {
                            UserId = user.Id,  // Fixed: use 'user' instead of 'superAdminUser'
                            RoleId = superAdminRole.Id,
                            TenantId = user.TenantId,
                            AssignedAt = DateTime.UtcNow,
                            AssignedBy = "System Seeder"
                        };

                        await _context.UserRoles.AddAsync(userRole);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
