using Identity.Domain.Entities;

namespace Identity.Application.Contracts.Persistence
{
    public interface IRolePermissionRepository
    {
        Task<List<Permission>> GetAllPermissionsAsync();
        Task AssignPermissionsToRoleAsync(string roleId, List<string> permissionIds, string tenantId);

        // ── New: fine-grained role-permission management ──────────────────────────
        Task<List<RolePermission>> GetRolePermissionsAsync(string roleId, string tenantId);
        Task<bool> RoleHasPermissionAsync(string roleId, string permissionId, string tenantId);
        Task AddPermissionToRoleAsync(string roleId, string permissionId, string tenantId);
        Task RemovePermissionFromRoleAsync(string roleId, string permissionId, string tenantId);
        Task ReplaceRolePermissionsAsync(string roleId, List<string> permissionIds, string tenantId);
    }
}
