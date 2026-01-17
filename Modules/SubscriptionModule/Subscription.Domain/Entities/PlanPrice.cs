using Subscription.Domain.Enums;
using System;
using SharedKernel.Enums;

namespace Subscription.Domain.Entities
{
    /// <summary>
    /// Multi-currency pricing for subscription plans.
    /// Supports different prices per currency and billing interval.
    /// </summary>
    public class PlanPrice
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string PlanId { get; set; } = string.Empty;
        
        // Pricing
        public string CurrencyCode { get; set; } = "USD"; // ISO 4217: "USD", "EGP", "SAR", "EUR"
        public decimal Amount { get; set; }
        public BillingInterval Interval { get; set; }
        
        // Metadata
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        
        // Navigation
        public virtual SubscriptionPlan Plan { get; set; } = null!;
    }
}
