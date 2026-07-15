using MediatR;
using SharedKernel.Enums;
using Subscription.Domain.Enums;

namespace Subscription.Application.Features.Payments.Commands.InitiatePayment
{
    public class InitiatePaymentCommand : IRequest<InitiatePaymentResponse>
    {
        public string TenantId { get; set; } = string.Empty;
        public PaymentPurpose Purpose { get; set; }
        public string TargetId { get; set; } = string.Empty;
        public long ExpectedAmountCents { get; set; }
        public string CurrencyCode { get; set; } = "EGP";
        public BillingInterval Interval { get; set; } = BillingInterval.Monthly;
        
        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
    }

    public class InitiatePaymentResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? PaymentId { get; set; }
        public string? ClientSecret { get; set; }
        public string? CheckoutUrl { get; set; }
        public string? ReferenceId { get; set; }
        public string? PublicKey { get; set; }
        public string? Status { get; set; }
        public bool IsReused { get; set; }
    }
}
