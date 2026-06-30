using Identity.Application.Contracts.Persistence;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Identity.Domain.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Services
{
    public class TenantRoleProvisioningService : ITenantRoleProvisioningService
    {
        private readonly IModuleRoleMappingService _moduleRoleMapping;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPermissionSynchronizationService _permissionSync;
        private readonly ILogger<TenantRoleProvisioningService> _logger;
        private readonly IPermissionRepository _permissionRepository;

        public TenantRoleProvisioningService(
            IModuleRoleMappingService moduleRoleMapping,
            RoleManager<ApplicationRole> roleManager,
            IPermissionSynchronizationService permissionSync,
            IPermissionRepository permissionRepository,
            ILogger<TenantRoleProvisioningService> logger)
        {
            _moduleRoleMapping = moduleRoleMapping;
            _roleManager = roleManager;
            _permissionSync = permissionSync;
            _permissionRepository = permissionRepository;
            _logger = logger;
        }

        public async Task<RoleProvisioningResult> ProvisionRolesAsync(
            string tenantId,
            List<string> effectiveModuleCodes)
        {
            // Step 1: Validate all module codes have a mapping
            var requiredRoles = new Dictionary<string, RoleMapping>(StringComparer.OrdinalIgnoreCase);

            foreach (var moduleCode in effectiveModuleCodes)
            {
                if (!_moduleRoleMapping.TryGetRoleMapping(moduleCode, out var mapping))
                {
                    return new RoleProvisioningResult
                    {
                        Success = false,
                        Error = $"Configuration error: module '{moduleCode}' has no role mapping. " +
                                "Contact system administrator to add the mapping."
                    };
                }

                requiredRoles[mapping.RoleName] = mapping;
            }

            // Step 2: Build roleToModule mapping for permission sync
            var roleToModuleMapping = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

            // SuperAdmin is always added
            roleToModuleMapping["SuperAdmin"] = null;

            foreach (var kvp in requiredRoles)
            {
                roleToModuleMapping[kvp.Key] = effectiveModuleCodes
                    .FirstOrDefault(mc =>
                        _moduleRoleMapping.TryGetRoleMapping(mc, out var rm) &&
                        string.Equals(rm.RoleName, kvp.Key, StringComparison.OrdinalIgnoreCase));
            }

            // Step 3: Get existing roles for the tenant
            var existingRoles = _roleManager.Roles
                .Where(r => r.TenantId == tenantId)
                .ToList();

            var existingRoleNames = existingRoles
                .Select(r => r.Name.ToCleanRoleName(tenantId))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // Step 4: Create missing roles
            var createdRoles = new List<ApplicationRole>();

            // Create SuperAdmin if not exists
            if (!existingRoleNames.Contains("SuperAdmin"))
            {
                var superAdminRole = CreateRole("SuperAdmin", RoleScope.ERP, tenantId);
                var result = await _roleManager.CreateAsync(superAdminRole);
                if (!result.Succeeded)
                {
                    return new RoleProvisioningResult
                    {
                        Success = false,
                        Error = $"Failed to create role SuperAdmin: {string.Join(", ", result.Errors.Select(e => e.Description))}"
                    };
                }
                createdRoles.Add(superAdminRole);
            }

            // Create module roles if not exist
            foreach (var kvp in requiredRoles)
            {
                if (existingRoleNames.Contains(kvp.Key))
                    continue;

                var role = CreateRole(kvp.Value.RoleName, kvp.Value.Scope, tenantId);
                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    return new RoleProvisioningResult
                    {
                        Success = false,
                        Error = $"Failed to create role {kvp.Key}: {string.Join(", ", result.Errors.Select(e => e.Description))}"
                    };
                }
                createdRoles.Add(role);
            }

            // Step 5: Sync permissions with explicit mapping
            await _permissionSync.SyncTenantPermissionsAsync(
                tenantId, effectiveModuleCodes, roleToModuleMapping);

            return new RoleProvisioningResult
            {
                Success = true,
                CreatedRoles = createdRoles
            };
        }

        private static ApplicationRole CreateRole(string roleName, RoleScope scope, string tenantId)
        {
            var tenantRoleName = $"{roleName}_{tenantId}";
            return new ApplicationRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = tenantRoleName,
                NormalizedName = tenantRoleName.ToUpper(),
                TenantId = tenantId,
                Scope = scope
            };
        }
    }
}
