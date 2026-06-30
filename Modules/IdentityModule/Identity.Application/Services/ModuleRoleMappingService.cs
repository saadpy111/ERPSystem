using Identity.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Services
{
    public class ModuleRoleMappingService : IModuleRoleMappingService
    {
        private readonly ILogger<ModuleRoleMappingService> _logger;

        private static readonly Dictionary<string, RoleMapping> Mappings = new(StringComparer.OrdinalIgnoreCase)
        {
            ["HR"] = new() { RoleName = "HRManager", Scope = RoleScope.ERP },
            ["INVENTORY"] = new() { RoleName = "InventoryManager", Scope = RoleScope.ERP },
            ["PROCUREMENT"] = new() { RoleName = "ProcurementManager", Scope = RoleScope.ERP },
            ["ACCOUNTING"] = new() { RoleName = "AccountingManager", Scope = RoleScope.ERP },
            ["CRM"] = new() { RoleName = "CRMManager", Scope = RoleScope.ERP },
            ["REPORT"] = new() { RoleName = "ReportManager", Scope = RoleScope.ERP },
            ["WEBSITE"] = new() { RoleName = "WebsiteManager", Scope = RoleScope.Website },
        };

        private static readonly HashSet<string> SuperAdminNames = new(StringComparer.OrdinalIgnoreCase)
        {
            "SuperAdmin"
        };

        public ModuleRoleMappingService(ILogger<ModuleRoleMappingService> logger)
        {
            _logger = logger;
        }

        public bool TryGetRoleMapping(string moduleCode, out RoleMapping mapping)
        {
            if (Mappings.TryGetValue(moduleCode, out mapping!))
                return true;

            _logger.LogError(
                "No role mapping configured for module '{ModuleCode}'. " +
                "This module was returned by subscription as enabled, but no role mapping exists.",
                moduleCode);

            return false;
        }

        public bool IsSuperAdmin(string cleanRoleName)
        {
            return SuperAdminNames.Contains(cleanRoleName);
        }

        public string? GetModuleCode(string cleanRoleName)
        {
            if (IsSuperAdmin(cleanRoleName))
                return null;

            foreach (var kvp in Mappings)
            {
                if (string.Equals(kvp.Value.RoleName, cleanRoleName, StringComparison.OrdinalIgnoreCase))
                    return kvp.Key;
            }

            return null;
        }

        public IReadOnlyDictionary<string, RoleMapping> GetAllMappings()
        {
            return Mappings;
        }
    }
}
