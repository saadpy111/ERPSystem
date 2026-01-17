using Subscription.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Subscription.Domain.Entities
{
    /// <summary>
    /// Tenant's active subscription with quota tracking.
    /// One active subscription per tenant.
    /// Note: Tenant entity lives in Identity module, referenced by TenantId only.
    /// </summary>
    public class TenantSubscription
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string TenantId { get; set; } = string.Empty;
        public string PlanId { get; set; } = string.Empty;
        
        // Lifecycle
        public SubscriptionStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? TrialEndsAt { get; set; }
        public DateTime CurrentPeriodStart { get; set; }
        public DateTime CurrentPeriodEnd { get; set; }
        public bool AutoRenew { get; set; } = true;
        
        // Billing
        public string BillingCycle { get; set; } = "monthly"; // "monthly", "yearly", "quarterly"
        public int BillingAnchorDay { get; set; } // Day of month for quota reset (1-28)
        public string CurrencyCode { get; set; } = "USD"; // Tenant's currency
        public string? ExternalSubscriptionId { get; set; } // Stripe/Paymob subscription ID
        public string? ExternalCustomerId { get; set; }
        
        // Quota Tracking
        public int CurrentUsers { get; set; }
        public long CurrentStorageBytes { get; set; }
        public int CurrentProducts { get; set; }
        public int CurrentMonthTransactions { get; set; } // Resets monthly
        public int CurrentMonthApiCalls { get; set; } // Resets monthly
        public DateTime LastQuotaResetAt { get; set; }
        
        // Metadata
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? CanceledAt { get; set; }
        public string? CancelReason { get; set; }
        
        // Navigation
        // Note: No direct navigation to Tenant (cross-module boundary)
        public virtual SubscriptionPlan Plan { get; set; } = null!;
        public virtual ICollection<SubscriptionHistory> History { get; set; } = new List<SubscriptionHistory>();
        public virtual ICollection<UsageHistory> UsageHistory { get; set; } = new List<UsageHistory>();
    }
}
