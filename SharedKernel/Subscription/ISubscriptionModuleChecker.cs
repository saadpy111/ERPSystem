using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedKernel.Subscription
{
    /// <summary>
    /// Cross-module contract for checking subscription module access.
    /// Consumed by Identity module (PermissionAuthorizationHandler, ModuleAccessMiddleware).
    /// Implemented by Subscription module.
    /// </summary>
    public interface ISubscriptionModuleChecker
    {
        /// <summary>
        /// Checks if a module is enabled in the tenant's active subscription.
        /// Returns false if no active subscription or module not included in plan.
        /// </summary>
        Task<bool> IsModuleEnabledAsync(string tenantId, string moduleName);

        /// <summary>
        /// Gets all enabled modules for a tenant.
        /// Used by authorization middleware and caching.
        /// </summary>
        Task<List<string>> GetEnabledModulesAsync(string tenantId);

        /// <summary>
        /// Validates if tenant has an active subscription.
        /// </summary>
        Task<bool> HasActiveSubscriptionAsync(string tenantId);

        /// <summary>
        /// Gets tenant's subscription status.
        /// </summary>
        Task<SubscriptionStatusDto> GetSubscriptionStatusAsync(string tenantId);
    }

    public class SubscriptionStatusDto
    {
        public bool IsActive { get; set; }
        public string Status { get; set; } = string.Empty; // "Trial", "Active", "Suspended", etc.
        public DateTime? ExpiresAt { get; set; }
        public string PlanName { get; set; } = string.Empty;
    }
}
