using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharedKernel.Enums;

namespace SharedKernel.Subscription
{
    /// <summary>
    /// Service for creating subscriptions when tenants are created.
    /// Consumed by Identity module (CreateCompanyCommandHandler).
    /// Implemented by Subscription module.
    /// </summary>
    public interface ISubscriptionService
    {
        /// <summary>
        /// Creates subscription for new tenant with user-selected plan.
        /// Called from CreateCompanyCommandHandler.
        /// </summary>
        /// <param name="tenantId">Tenant identifier</param>
        /// <param name="tenantName">Tenant name for history tracking</param>
        /// <param name="planCodeOrId">User-selected plan code (e.g., "BUSINESS") or ID</param>
        /// <param name="currencyCode">Selected currency (e.g., "USD", "EGP")</param>
        /// <param name="interval">Billing interval (Monthly, Yearly, Quarterly)</param>
        Task<CreateSubscriptionResult> CreateSubscriptionAsync(
            string tenantId,
            string tenantName,
            string planCodeOrId,
            string currencyCode,
            BillingInterval interval);
    }

    public class CreateSubscriptionResult
    {
        public bool Success { get; set; }
        public string? SubscriptionId { get; set; }
        public string? Error { get; set; }
        public List<string> EnabledModules { get; set; } = new();
        
        // Plan information
        public string? PlanCode { get; set; }
        public string? PlanName { get; set; }
        public bool IsTrial { get; set; }
        public DateTime? TrialEndsAt { get; set; }
    }
}
