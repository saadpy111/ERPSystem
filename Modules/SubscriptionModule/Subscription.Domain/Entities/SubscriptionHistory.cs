using Subscription.Domain.Enums;
using System;

namespace Subscription.Domain.Entities
{
    /// <summary>
    /// Audit trail of subscription lifecycle events.
    /// Tracks all changes for compliance and debugging.
    /// </summary>
    public class SubscriptionHistory
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string TenantSubscriptionId { get; set; } = string.Empty;
        
        public SubscriptionEventType EventType { get; set; }
        public string? FromPlanId { get; set; }
        public string? ToPlanId { get; set; }
        public string? FromStatus { get; set; }
        public string? ToStatus { get; set; }
        
        public string Notes { get; set; } = string.Empty;
        public string PerformedBy { get; set; } = "System";
        public DateTime CreatedAt { get; set; }
        
        // Navigation
        public virtual TenantSubscription Subscription { get; set; } = null!;
    }
}
