using MediatR;

namespace Subscription.Application.Features.SubscriptionRenewalPayment.Commands.CreateSubscriptionRenewalPayment
{
    public sealed class CreateSubscriptionRenewalPaymentCommand : IRequest<CreateSubscriptionRenewalPaymentResponse>
    {
        public string TenantId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;

        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
    }

    public sealed class CreateSubscriptionRenewalPaymentResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? PaymentId { get; set; }
        public string? ClientSecret { get; set; }
        public string? CheckoutUrl { get; set; }
        public string? ReferenceId { get; set; }
        public string? PublicKey { get; set; }
        public string? Status { get; set; }
    }
}
