using Identity.Domain.Entities;
using Identity.Persistense.Context;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Constants.Permissions;

namespace Identity.Persistense.Seeders
{
    public class PermissionSeeder
    {
        private readonly IdentityDbContext _context;

        public PermissionSeeder(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Check if ANY permissions exist globally
            if (await _context.Permissions.AnyAsync())
            {
                return; // Already seeded
            }

            // Get all permissions from SharedKernel using reflection
            var permissionsByModule = Permissions.GetPermissionsByModule();
            var permissionEntities = new List<Permission>();

            foreach (var module in permissionsByModule)
            {
                foreach (var permissionName in module.Value)
                {
                    permissionEntities.Add(new Permission
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = permissionName,
                        Module = module.Key,
                        Description = GenerateDescription(permissionName),
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            await _context.Permissions.AddRangeAsync(permissionEntities);
            await _context.SaveChangesAsync();
        }

        private string GenerateDescription(string permissionName)
        {
            // Generate human-readable description from permission name
            // e.g., "Inventory.Products.View" => "View products in inventory"
            var parts = permissionName.Split('.');
            if (parts.Length >= 3)
            {
                var action = parts[2];
                var resource = parts[1];
                var module = parts[0];
                return $"{action} {resource.ToLower()} in {module.ToLower()}";
            }
            return permissionName;
        }
    }
}
