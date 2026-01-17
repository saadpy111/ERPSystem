using Microsoft.AspNetCore.Authorization;
using SharedKernel.Subscription;

namespace SharedKernel.Authorization
{
    /// <summary>
    /// Handles PermissionRequirement by checking INTERSECTION:
    /// 1. User has permission (from claims)
    /// 2. AND Module is enabled in tenant's subscription
    /// </summary>
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly ISubscriptionModuleChecker _subscriptionChecker;
        private readonly IPermissionModuleMapper _permissionMapper;

        public PermissionAuthorizationHandler(
            ISubscriptionModuleChecker subscriptionChecker,
            IPermissionModuleMapper permissionMapper)
        {
            _subscriptionChecker = subscriptionChecker;
            _permissionMapper = permissionMapper;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            // Auto-deny if no tenant (pre-tenant users cannot access business logic)
            var tenantId = context.User.FindFirst("tenant")?.Value;
            if (string.IsNullOrEmpty(tenantId))
            {
                context.Fail();
                return;
            }

            // Step 1: Check if user has the permission (from claims)
            var permissionClaims = context.User.FindAll("permission").Select(c => c.Value).ToList();
            
            if (!permissionClaims.Contains(requirement.Permission))
            {
                context.Fail();
                return;
            }

            // Step 2: INTERSECTION - Check if module is enabled in subscription
            var permissionModule = _permissionMapper.GetModuleForPermission(requirement.Permission);
            
            if (permissionModule != null)
            {
                var isModuleEnabled = await _subscriptionChecker.IsModuleEnabledAsync(tenantId, permissionModule);
                
                if (!isModuleEnabled)
                {
                    context.Fail(); // Permission exists but module disabled in subscription
                    return;
                }
            }

            // Both checks passed: user has permission AND module is enabled
            context.Succeed(requirement);
        }
    }
}
