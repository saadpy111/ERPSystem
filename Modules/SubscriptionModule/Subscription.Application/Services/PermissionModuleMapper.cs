using SharedKernel.Subscription;
using System.Collections.Generic;
using System.Linq;

namespace Subscription.Application.Services
{
    /// <summary>
    /// Implements IPermissionModuleMapper by parsing permission names.
    /// Maps permissions to their owning modules for intersection authorization.
    /// Example: "Inventory.Products.View" → "Inventory"
    /// </summary>
    public class PermissionModuleMapper : IPermissionModuleMapper
    {
        // Known module prefixes
        private static readonly HashSet<string> KnownModules = new()
        {
            "Inventory",
            "HR",
            "CRM",
            "Accounting",
            "Ecommerce"
        };

        public string? GetModuleForPermission(string permissionName)
        {
            if (string.IsNullOrWhiteSpace(permissionName))
                return null;

            // Permission format: "Module.Feature.Action"
            // Example: "Inventory.Products.View"
            var parts = permissionName.Split('.');
            
            if (parts.Length < 2)
                return null; // Not a module-scoped permission

            var potentialModule = parts[0];

            // Verify it's a known module
            if (KnownModules.Contains(potentialModule))
                return potentialModule;

            return null; // Global permission (not module-scoped)
        }
    }
}
