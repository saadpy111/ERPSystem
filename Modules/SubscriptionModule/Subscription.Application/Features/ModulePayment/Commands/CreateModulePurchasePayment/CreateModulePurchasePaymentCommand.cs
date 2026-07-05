using MediatR;
using SharedKernel.Enums;

namespace Subscription.Application.Features.ModulePayment.Commands.CreateModulePurchasePayment
{
    public sealed class CreateModulePurchasePaymentCommand : IRequest<CreateModulePurchasePaymentResponse>
    {
        public string TenantId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string ModuleCode { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public BillingInterval Interval { get; set; }

        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
    }

    public sealed class CreateModulePurchasePaymentResponse
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
