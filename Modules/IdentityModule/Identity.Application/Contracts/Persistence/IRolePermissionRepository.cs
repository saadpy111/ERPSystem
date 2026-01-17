using Identity.Domain.Entities;

namespace Identity.Application.Contracts.Persistence
{
    public interface IRolePermissionRepository
    {
        Task AssignPermissionsToRoleAsync(string roleId, List<string> permissionIds, string tenantId);
        Task<List<Permission>> GetAllPermissionsAsync();
    }
}
