namespace SharedKernel.Subscription
{
    /// <summary>
    /// Maps permissions to their owning modules.
    /// Used for intersection authorization logic.
    /// Implemented by Subscription module based on Permission.Module field.
    /// </summary>
    public interface IPermissionModuleMapper
    {
        /// <summary>
        /// Gets the module name for a given permission.
        /// Returns null for global permissions.
        /// Example: "Inventory.Products.View" → "Inventory"
        /// </summary>
        string? GetModuleForPermission(string permissionName);
    }
}
