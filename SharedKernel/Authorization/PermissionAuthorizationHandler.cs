using Microsoft.AspNetCore.Authorization;
using SharedKernel.Subscription;

namespace SharedKernel.Authorization
{
    /// <summary>
    /// Handles PermissionRequirement using a hybrid authorization model:
    ///
    ///   1. Tenant guard      – deny immediately if "tenant" claim is absent.
    ///   2. Hybrid permission – delegate to <see cref="IPermissionService"/>:
    ///                            a. Check JWT "permission" claims (fast).
    ///                            b. Fall back to DB lookup if not in token.
    ///   3. Subscription gate – deny if the owning module is disabled in the
    ///                          tenant's active subscription plan (INTERSECTION).
    ///
    /// The handler itself has NO direct dependency on persistence or HTTP context.
    /// All permission resolution is handled by IPermissionService.
    /// </summary>
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionService _permissionService;
        private readonly ISubscriptionModuleChecker _subscriptionChecker;
        private readonly IPermissionModuleMapper _permissionMapper;

        public PermissionAuthorizationHandler(
            IPermissionService permissionService,
            ISubscriptionModuleChecker subscriptionChecker,
            IPermissionModuleMapper permissionMapper)
        {
            _permissionService = permissionService;
            _subscriptionChecker = subscriptionChecker;
            _permissionMapper = permissionMapper;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            // ── Guard: tenant claim must be present ──────────────────────────────
            // Pre-tenant users (e.g. during onboarding) must never reach business logic.
            var tenantId = context.User.FindFirst("tenant")?.Value;
            if (string.IsNullOrEmpty(tenantId))
            {
                context.Fail();
                return;
            }

            // ── Hybrid permission check (token → DB fallback) ────────────────────
            // IPermissionService encapsulates all logic; the handler stays clean.
            var hasPermission = await _permissionService
                .HasPermissionAsync(context.User, requirement.Permission);

            if (!hasPermission)
            {
                context.Fail();
                return;
            }

            // ── Subscription intersection: module must be enabled ────────────────
            var permissionModule = _permissionMapper.GetModuleForPermission(requirement.Permission);

            if (permissionModule != null)
            {
                var isModuleEnabled = await _subscriptionChecker
                    .IsModuleEnabledAsync(tenantId, permissionModule);

                if (!isModuleEnabled)
                {
                    context.Fail(); // Permission granted but module disabled in plan
                    return;
                }
            }

            // All checks passed
            context.Succeed(requirement);
        }
    }
}
