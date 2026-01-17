using System;

namespace Subscription.Domain.Entities
{
    /// <summary>
    /// Tracks quota usage snapshots for auditing and overage billing.
    /// Created by QuotaResetBackgroundJob before resetting quotas.
    /// </summary>
    public class UsageHistory
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string TenantSubscriptionId { get; set; } = string.Empty;
        
        // Usage snapshot (before reset)
        public int Users { get; set; }
        public long StorageBytes { get; set; }
        public int Products { get; set; }
        public int Transactions { get; set; }
        public int ApiCalls { get; set; }
        
        // Period tracking
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public DateTime SnapshotAt { get; set; }
        
        // Overage detection
        public bool HasOverage { get; set; }
        public string? OverageDetails { get; set; } // JSON: {"transactions": 150, "limit": 100}
        
        // Navigation
        public virtual TenantSubscription Subscription { get; set; } = null!;
    }
}
