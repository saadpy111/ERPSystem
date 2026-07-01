using Identity.Application.Contracts.Persistence;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Identity.Domain.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Subscription;

namespace Identity.Application.Services
{
    public class TenantRoleProvisioningService :
        ITenantRoleProvisioningService,
        SharedKernel.Subscription.ITenantRoleProvisioningService
    {
        private readonly IModuleRoleMappingService _moduleRoleMapping;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPermissionSynchronizationService _permissionSync;
        private readonly ILogger<TenantRoleProvisioningService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPermissionRepository _permissionRepository;

        public TenantRoleProvisioningService(
            IModuleRoleMappingService moduleRoleMapping,
            RoleManager<ApplicationRole> roleManager,
            IPermissionSynchronizationService permissionSync,
            IPermissionRepository permissionRepository,
            ILogger<TenantRoleProvisioningService> logger,
            IUnitOfWork unitOfWork)
        {
            _moduleRoleMapping = moduleRoleMapping;
            _roleManager = roleManager;
            _permissionSync = permissionSync;
            _permissionRepository = permissionRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<RoleProvisioningResult> ProvisionRolesAsync(
            string tenantId,
            List<string> effectiveModuleCodes)
        {
            return await ProvisionRolesInternalAsync(tenantId, effectiveModuleCodes);
        }

        async Task<SharedKernel.Subscription.RoleProvisioningResult> SharedKernel.Subscription.ITenantRoleProvisioningService.ProvisionRolesAsync(
            string tenantId,
            List<string> effectiveModuleCodes)
        {
            var result = await ProvisionRolesInternalAsync(tenantId, effectiveModuleCodes);
            return new SharedKernel.Subscription.RoleProvisioningResult
            {
                Success = result.Success,
                Error = result.Error
            };
        }

        private async Task<RoleProvisioningResult> ProvisionRolesInternalAsync(
            string tenantId,
            List<string> effectiveModuleCodes)
        {
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

            var roleToModuleMapping = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

            roleToModuleMapping["SuperAdmin"] = null;

            foreach (var kvp in requiredRoles)
            {
                roleToModuleMapping[kvp.Key] = effectiveModuleCodes
                    .FirstOrDefault(mc =>
                        _moduleRoleMapping.TryGetRoleMapping(mc, out var rm) &&
                        string.Equals(rm.RoleName, kvp.Key, StringComparison.OrdinalIgnoreCase));
            }

            var existingRoles = _roleManager.Roles
                .Where(r => r.TenantId == tenantId)
                .ToList();

            var existingRoleNames = existingRoles
                .Select(r => r.Name.ToCleanRoleName(tenantId))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var createdRoles = new List<ApplicationRole>();

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

            await _permissionSync.SyncTenantPermissionsAsync(
                tenantId, effectiveModuleCodes, roleToModuleMapping);
            await _unitOfWork.SaveChangesAsync();
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
