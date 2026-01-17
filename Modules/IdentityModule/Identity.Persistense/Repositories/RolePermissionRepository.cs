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
        {
            // Get all global permissions (no tenant filter)
            return await _context.Permissions.ToListAsync();
        }

        public async Task AssignPermissionsToRoleAsync(string roleId, List<string> permissionIds, string tenantId)
        {
            var rolePermissions = permissionIds.Select(permissionId => new RolePermission
            {
                Id = Guid.NewGuid().ToString(),
                RoleId = roleId,
                PermissionId = permissionId,
                TenantId = tenantId,
                AssignedAt = DateTime.UtcNow
            }).ToList();

            await _context.RolePermissions.AddRangeAsync(rolePermissions);
        }
    }
}
