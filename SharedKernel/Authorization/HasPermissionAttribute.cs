using Microsoft.AspNetCore.Authorization;

namespace SharedKernel.Authorization
{
    /// <summary>
    /// Authorization attribute that sets the policy name to the required permission.
    /// Follows standard ASP.NET Core pattern by inheriting from AuthorizeAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(string permission)
        {
            Policy = permission; // Policy name = permission constant
        }
    }
}
