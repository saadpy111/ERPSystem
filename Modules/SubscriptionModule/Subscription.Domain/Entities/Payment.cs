using SharedKernel.Enums;
using Subscription.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Subscription.Domain.Entities
{
    public class Payment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? TenantId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public PaymentPurpose Purpose { get; set; }
        public string TargetId { get; set; } = string.Empty;
        public long ExpectedAmountCents { get; set; }
        public string CurrencyCode { get; set; } = "EGP";
        public BillingInterval Interval { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public long? GatewayOrderId { get; set; }
        public string? ClientSecret { get; set; }
        public string? CheckoutUrl { get; set; }
        
        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;

        public string? Payload { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; }

        public virtual ICollection<PaymentTransaction> Transactions { get; set; } = new List<PaymentTransaction>();
    }
}
