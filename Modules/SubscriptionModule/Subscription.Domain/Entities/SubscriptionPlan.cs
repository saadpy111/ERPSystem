using System;
using System.Collections.Generic;

namespace Subscription.Domain.Entities
{
    /// <summary>
    /// Defines subscription plan tiers with quotas and module access.
    /// Does NOT contain pricing - see PlanPrice entity for multi-currency support.
    /// </summary>
    public class SubscriptionPlan
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Code { get; set; } = string.Empty; // "STARTER", "BUSINESS", "ENTERPRISE"
        public string Name { get; set; } = string.Empty; // "Free", "Trial", "Basic", "Pro", "Enterprise"
        public string DisplayName { get; set; } = string.Empty; // "Professional Plan"
        public string Description { get; set; } = string.Empty;
        
        // Trial configuration
        public bool IsTrial { get; set; }
        public int TrialDays { get; set; } // e.g., 14
        
        // Plan visibility
        public bool IsVisible { get; set; } = true; // Hidden legacy plans
        public int SortOrder { get; set; }
        
        // Quota Limits (-1 = unlimited)
        public int MaxUsers { get; set; } = -1;
        public long MaxStorageBytes { get; set; } = -1;
        public int MaxProducts { get; set; } = -1;
        public int MaxMonthlyTransactions { get; set; } = -1;
        public int MaxMonthlyApiCalls { get; set; } = -1;
        
        // Metadata
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Navigation Properties
        public virtual ICollection<PlanModule> PlanModules { get; set; } = new List<PlanModule>();
        public virtual ICollection<PlanPrice> Prices { get; set; } = new List<PlanPrice>();
        public virtual ICollection<TenantSubscription> TenantSubscriptions { get; set; } = new List<TenantSubscription>();
    }
}
