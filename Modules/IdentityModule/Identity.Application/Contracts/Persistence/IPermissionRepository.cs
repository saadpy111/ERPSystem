namespace Identity.Application.Contracts.Persistence
{
    public interface IPermissionRepository
    {
        /// <summary>
        /// Returns all effective permission names for a user within a specific tenant.
        /// Combines direct user permissions and role-based permissions,
        /// always filtered by tenantId for strict multi-tenant isolation.
        /// </summary>
        Task<List<string>> GetUserEffectivePermissionsAsync(string userId, string tenantId);
    }
}
