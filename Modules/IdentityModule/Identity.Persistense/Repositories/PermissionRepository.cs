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

        /// <inheritdoc />
        public async Task<List<string>> GetUserEffectivePermissionsAsync(string userId, string tenantId)
        {
            // IgnoreQueryFilters so we control isolation manually — the DbContext
            // query filter may not have been built for this tenantId at startup.

            // 1. Direct user permissions (scoped to the tenant)
            var userPermissions = await _context.UserPermissions
                .IgnoreQueryFilters()
                .Where(up => up.UserId == userId && up.TenantId == tenantId)
                .Include(up => up.Permission)
                .Select(up => up.Permission.Name)
                .ToListAsync();

            // 2. Role-based permissions (roles scoped to the tenant)
            var rolePermissions = await _context.UserRoles
                .IgnoreQueryFilters()
                .Where(ur => ur.UserId == userId && ur.TenantId == tenantId)
                .Join(
                    _context.RolePermissions.IgnoreQueryFilters()
                        .Where(rp => rp.TenantId == tenantId),
                    ur => ur.RoleId,
                    rp => rp.RoleId,
                    (ur, rp) => rp)
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission.Name)
                .ToListAsync();

            // Merge and deduplicate
            return userPermissions.Union(rolePermissions).Distinct().ToList();
        }
    }
}
