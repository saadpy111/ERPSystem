using Microsoft.AspNetCore.Authorization;

namespace SharedKernel.Authorization
{
    /// <summary>
    /// Authorization requirement that encapsulates a permission check.
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
