using SharedKernel.Enums;
using Subscription.Domain.Enums;
using System;

namespace Subscription.Domain.Entities
{
    public class TenantModuleSubscription
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string TenantId { get; set; } = string.Empty;
        public string ModuleId { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }
        public string CurrencyCode { get; set; } = "USD";
        public BillingInterval Interval { get; set; }

        public ModuleSubscriptionStatus Status { get; set; } = ModuleSubscriptionStatus.Active;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool AutoRenew { get; set; } = true;
        public string? ExternalSubscriptionId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Module Module { get; set; } = null!;
    }
}
