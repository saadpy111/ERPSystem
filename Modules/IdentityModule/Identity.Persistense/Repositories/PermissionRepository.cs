using Identity.Application.Contracts.Persistence;
using Identity.Persistense.Context;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistense.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly IdentityDbContext _context;

        public PermissionRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetUserEffectivePermissionsAsync(string userId)
        {
            var permissions = new List<string>();

            // Get direct user permissions
            var userPermissions = await _context.UserPermissions
                 .IgnoreQueryFilters()
                .Where(up => up.UserId == userId)
                .Include(up => up.Permission)
                .Select(up => up.Permission.Name)
                .ToListAsync();

            permissions.AddRange(userPermissions);

            // Get permissions from user's roles
            var rolePermissions = await _context.UserRoles
                 .IgnoreQueryFilters()
                .Where(ur => ur.UserId == userId)
                .Join(_context.RolePermissions,
                    ur => ur.RoleId,
                    rp => rp.RoleId,
                    (ur, rp) => rp)
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission.Name)
                .ToListAsync();

            permissions.AddRange(rolePermissions);

            // Return distinct permissions
            return permissions.Distinct().ToList();
        }
    }
}
