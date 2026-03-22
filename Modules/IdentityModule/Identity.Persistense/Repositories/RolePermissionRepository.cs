using Identity.Application.Contracts.Persistence;
using Identity.Domain.Entities;
using Identity.Persistense.Context;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistense.Repositories
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly IdentityDbContext _context;

        public RolePermissionRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<List<Permission>> GetAllPermissionsAsync()
            => await _context.Permissions.ToListAsync();

        public async Task AssignPermissionsToRoleAsync(string roleId, List<string> permissionIds, string tenantId)
        {
            var rolePermissions = permissionIds.Select(permissionId => new RolePermission
            {
                Id           = Guid.NewGuid().ToString(),
                RoleId       = roleId,
                PermissionId = permissionId,
                TenantId     = tenantId,
                AssignedAt   = DateTime.UtcNow
            }).ToList();

            await _context.RolePermissions.AddRangeAsync(rolePermissions);
        }

        public async Task<List<RolePermission>> GetRolePermissionsAsync(string roleId, string tenantId)
            => await _context.RolePermissions
                .IgnoreQueryFilters()
                .Where(rp => rp.RoleId == roleId && rp.TenantId == tenantId)
                .Include(rp => rp.Permission)
                .ToListAsync();

        public async Task<bool> RoleHasPermissionAsync(string roleId, string permissionId, string tenantId)
            => await _context.RolePermissions
                .IgnoreQueryFilters()
                .AnyAsync(rp =>
                    rp.RoleId       == roleId       &&
                    rp.PermissionId == permissionId &&
                    rp.TenantId     == tenantId);

        public async Task AddPermissionToRoleAsync(string roleId, string permissionId, string tenantId)
        {
            var rp = new RolePermission
            {
                Id           = Guid.NewGuid().ToString(),
                RoleId       = roleId,
                PermissionId = permissionId,
                TenantId     = tenantId,
                AssignedAt   = DateTime.UtcNow
            };
            await _context.RolePermissions.AddAsync(rp);
        }

        public async Task RemovePermissionFromRoleAsync(string roleId, string permissionId, string tenantId)
        {
            var rp = await _context.RolePermissions
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(rp =>
                    rp.RoleId       == roleId       &&
                    rp.PermissionId == permissionId &&
                    rp.TenantId     == tenantId);

            if (rp != null) _context.RolePermissions.Remove(rp);
        }

        public async Task ReplaceRolePermissionsAsync(string roleId, List<string> permissionIds, string tenantId)
        {
            // Remove all existing
            var existing = await GetRolePermissionsAsync(roleId, tenantId);
            _context.RolePermissions.RemoveRange(existing);

            // Add new set
            if (permissionIds.Any())
                await AssignPermissionsToRoleAsync(roleId, permissionIds, tenantId);
        }
    }
}
