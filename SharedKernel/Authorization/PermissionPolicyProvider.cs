using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using SharedKernel.Constants.Permissions;

namespace SharedKernel.Authorization
{
    /// <summary>
    /// Dynamic policy provider that creates authorization policies on-the-fly for permissions.
    /// When a policy name matches a permission constant, it creates a policy with PermissionRequirement.
    /// </summary>
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;
        private static readonly HashSet<string> _allPermissions;

        static PermissionPolicyProvider()
        {
            // Cache all permissions at startup using reflection
            _allPermissions = new HashSet<string>(Permissions.GetAllPermissions());
        }

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
            => _fallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
            => _fallbackPolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // If policy name matches a permission constant, create dynamic policy
            if (_allPermissions.Contains(policyName))
            {
                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new PermissionRequirement(policyName))
                    .Build();

                return Task.FromResult<AuthorizationPolicy?>(policy);
            }

            // Otherwise, fall back to default provider (for role-based policies, etc.)
            return _fallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
