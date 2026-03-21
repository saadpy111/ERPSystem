namespace Identity.Application.Features.AuthFeature.Queries.GetMyPermissions
{
    /// <summary>
    /// Response for GET /api/me/permissions.
    /// Contains all effective permissions (direct + role-based) for the
    /// authenticated user within their tenant.
    /// </summary>
    public class GetMyPermissionsResponse
    {
        /// <summary>
        /// Full list of effective permission names.
        /// Example: ["Inventory.Products.View", "HR.Employees.Read"]
        /// </summary>
        public List<string> Permissions { get; init; } = new();
    }
}
