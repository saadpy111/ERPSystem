using System.Reflection;

namespace SharedKernel.Constants.Permissions
{
    /// <summary>
    /// Centralized permission entry point for the entire ERP system.
    /// 
    /// DESIGN:
    /// - Each module has its own permission file (InventoryPermissions, HrPermissions, etc.)
    /// - This class aggregates all permissions using reflection
    /// - Supports: Database seeding, authorization, subscription mapping
    /// 
    /// NAMING CONVENTION:
    /// Module.Entity.Action (e.g., "Inventory.Products.View")
    /// </summary>
    public static class Permissions
    {
        // All module permission types for reflection
        private static readonly Type[] ModuleTypes = new[]
        {
            typeof(InventoryPermissions),
            typeof(HrPermissions),
            typeof(ProcurementPermissions),
            typeof(AdminPermissions),
            typeof(WebsitePermissions),
            typeof(ReportPermissions),
            typeof(SubscriptionPermissions)
        };

        /// <summary>
        /// Gets all permission constants using reflection.
        /// Returns a flat list of all const string fields from all module permission classes.
        /// </summary>
        public static List<string> GetAllPermissions()
        {
            var permissions = new List<string>();

            foreach (var moduleType in ModuleTypes)
            {
                var fields = moduleType
                    .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string) && f.Name != "Module");

                foreach (var field in fields)
                {
                    var value = field.GetValue(null)?.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        permissions.Add(value);
                    }
                }
            }

            return permissions;
        }

        /// <summary>
        /// Gets all permissions grouped by module.
        /// Key: Module name (e.g., "Inventory")
        /// Value: List of permission strings
        /// </summary>
        public static Dictionary<string, List<string>> GetPermissionsByModule()
        {
            var result = new Dictionary<string, List<string>>();

            foreach (var moduleType in ModuleTypes)
            {
                var moduleField = moduleType.GetField("Module", BindingFlags.Public | BindingFlags.Static);
                var moduleName = moduleField?.GetValue(null)?.ToString() ?? moduleType.Name.Replace("Permissions", "");

                var fields = moduleType
                    .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string) && f.Name != "Module");

                var modulePermissions = new List<string>();
                foreach (var field in fields)
                {
                    var value = field.GetValue(null)?.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        modulePermissions.Add(value);
                    }
                }

                if (modulePermissions.Any())
                {
                    result[moduleName] = modulePermissions;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets all module names.
        /// </summary>
        public static List<string> GetAllModules()
        {
            return GetPermissionsByModule().Keys.ToList();
        }

        /// <summary>
        /// Gets permissions for a specific module.
        /// Returns empty list if module not found.
        /// </summary>
        public static List<string> GetPermissionsForModule(string moduleName)
        {
            var byModule = GetPermissionsByModule();
            return byModule.TryGetValue(moduleName, out var permissions) ? permissions : new List<string>();
        }

        /// <summary>
        /// Gets permissions for multiple modules.
        /// Used for subscription plan → modules → permissions mapping.
        /// </summary>
        public static List<string> GetPermissionsForModules(IEnumerable<string> moduleNames)
        {
            var byModule = GetPermissionsByModule();
            var result = new List<string>();

            foreach (var moduleName in moduleNames)
            {
                if (byModule.TryGetValue(moduleName, out var permissions))
                {
                    result.AddRange(permissions);
                }
            }

            return result.Distinct().ToList();
        }

        /// <summary>
        /// Checks if a permission string exists.
        /// </summary>
        public static bool IsValidPermission(string permission)
        {
            return GetAllPermissions().Contains(permission);
        }

        /// <summary>
        /// Extracts the module name from a permission string.
        /// E.g., "Inventory.Products.View" → "Inventory"
        /// </summary>
        public static string? GetModuleFromPermission(string permission)
        {
            if (string.IsNullOrEmpty(permission)) return null;
            var parts = permission.Split('.');
            return parts.Length > 0 ? parts[0] : null;
        }
    }
}
