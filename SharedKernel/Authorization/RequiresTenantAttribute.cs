using System;

namespace SharedKernel.Authorization
{
    /// <summary>
    /// Attribute to mark endpoints that require a user to have a tenant.
    /// Pre-tenant users (TenantId = NULL) will be automatically denied access.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequiresTenantAttribute : Attribute
    {
    }
}
