using Identity.Application.Contracts.Persistence;
using SharedKernel.Authorization;
using System.Security.Claims;

namespace Identity.Application.Services
{
    /// <summary>
    /// Hybrid permission-checking service.
    ///
    /// Strategy:
    ///   1. Fast path  → look for the permission directly in the JWT "permission" claims.
    ///   2. Fallback   → if the claim is absent, query the database using
    ///                   IPermissionRepository with strict tenant isolation.
    ///
    /// This design keeps the AuthorizationHandler in SharedKernel free from any
    /// infrastructure or persistence dependencies.
    /// </summary>
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        /// <inheritdoc />
        public async Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission)
        {
            // ── Fast path: check permission claims embedded in the JWT ──────────
            var tokenPermissions = user.FindAll("permission").Select(c => c.Value);

            if (tokenPermissions.Contains(permission))
                return true;

            // ── Fallback: query the database ─────────────────────────────────────
            // Extract identity claims — fail fast if either is missing.
            var userId =
                user.FindFirst("sub")?.Value ??
                user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var tenantId = user.FindFirst("tenant")?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(tenantId))
                return false;

            var dbPermissions = await _permissionRepository
                .GetUserEffectivePermissionsAsync(userId, tenantId);

            return dbPermissions.Contains(permission);
        }
    }
}
