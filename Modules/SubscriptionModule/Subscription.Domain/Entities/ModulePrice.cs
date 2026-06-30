using SharedKernel.Enums;
using System;

namespace Subscription.Domain.Entities
{
    public class ModulePrice
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ModuleId { get; set; } = string.Empty;

        public string CurrencyCode { get; set; } = "USD";
        public decimal UnitPrice { get; set; }
        public BillingInterval Interval { get; set; }

        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }

        public virtual Module Module { get; set; } = null!;
    }
}
