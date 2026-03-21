using System.Security.Claims;

namespace SharedKernel.Authorization
{
    /// <summary>
    /// Abstraction for hybrid permission checking.
    /// Checks the JWT token claims first (fast path),
    /// then falls back to a database lookup (consistent path).
    /// This keeps the AuthorizationHandler clean and decoupled from persistence.
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// Returns true if the given ClaimsPrincipal holds the requested permission.
        /// Strategy:
        ///   1. Check "permission" claims in the token → fast path.
        ///   2. If not found, extract userId ("sub") and tenantId ("tenant")
        ///      and query the database with tenant isolation → consistent fallback.
        /// </summary>
        /// <param name="user">The authenticated ClaimsPrincipal from the HTTP context.</param>
        /// <param name="permission">The permission name to verify (e.g. "Inventory.Products.View").</param>
        Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission);
    }
}
