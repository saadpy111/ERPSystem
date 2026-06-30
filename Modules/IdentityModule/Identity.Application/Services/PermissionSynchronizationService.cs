using Identity.Application.Contracts.Persistence;
using Identity.Domain.Entities;
using Identity.Domain.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Services
{
    public interface IPermissionSynchronizationService
    {
        Task SyncTenantPermissionsAsync(
            string tenantId,
            List<string> effectiveModules,
            IReadOnlyDictionary<string, string?> roleToModuleMapping);
    }

    public class PermissionSynchronizationService : IPermissionSynchronizationService
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public PermissionSynchronizationService(
            IRolePermissionRepository rolePermissionRepository,
            RoleManager<ApplicationRole> roleManager)
        {
            _rolePermissionRepository = rolePermissionRepository;
            _roleManager = roleManager;
        }

        public async Task SyncTenantPermissionsAsync(
            string tenantId,
            List<string> effectiveModules,
            IReadOnlyDictionary<string, string?> roleToModuleMapping)
        {
            var allPermissions = await _rolePermissionRepository.GetAllPermissionsAsync();

            var enabledPermissions = allPermissions
                .Where(p => string.IsNullOrEmpty(p.Module) || effectiveModules.Contains(p.Module, StringComparer.OrdinalIgnoreCase))
                .ToList();

            var adminPermissionIds = allPermissions
                .Where(p => p.Module == "Admin")
                .Select(p => p.Id)
                .ToList();

            var roles = _roleManager.Roles
                .Where(r => r.TenantId == tenantId)
                .ToList();

            foreach (var role in roles)
            {
                var cleanName = role.Name.ToCleanRoleName(tenantId);

                List<string> permissionIdsToAssign;

                if (roleToModuleMapping.TryGetValue(cleanName, out var mappedModule))
                {
                    if (mappedModule == null)
                    {
                        // SuperAdmin: all enabled permissions + Admin permissions
                        permissionIdsToAssign = enabledPermissions
                            .Select(p => p.Id)
                            .Union(adminPermissionIds)
                            .Distinct()
                            .ToList();
                    }
                    else
                    {
                        // Module-specific role: permissions matching that module only
                        permissionIdsToAssign = enabledPermissions
                            .Where(p => p.Module != null &&
                                   p.Module.Equals(mappedModule, StringComparison.OrdinalIgnoreCase))
                            .Select(p => p.Id)
                            .ToList();
                    }
                }
                else
                {
                    // Role not in mapping (manually created, no module association) — no permissions
                    permissionIdsToAssign = new List<string>();
                }

                if (permissionIdsToAssign.Count == 0)
                {
                    var existing = await _rolePermissionRepository.GetRolePermissionsAsync(role.Id, tenantId);
                    if (existing.Count > 0)
                        await _rolePermissionRepository.ReplaceRolePermissionsAsync(role.Id, new List<string>(), tenantId);
                    continue;
                }

                await _rolePermissionRepository.ReplaceRolePermissionsAsync(role.Id, permissionIdsToAssign, tenantId);
            }
        }
    }
}
