using Subscription.Domain.Enums;
using System;

namespace Subscription.Domain.Entities
{
    public class PaymentTransaction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string PaymentId { get; set; } = string.Empty;
        public string GatewayTransactionId { get; set; } = string.Empty;
        public string? PaymentMethod { get; set; }
        public long AmountCents { get; set; }
        public PaymentTransactionStatus Status { get; set; }
        public string? GatewayStatus { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        
        public DateTime WebhookReceivedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }

        // Navigation Property
        public virtual Payment Payment { get; set; } = null!;
    }
}
