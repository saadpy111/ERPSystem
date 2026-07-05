using MediatR;

namespace Subscription.Application.Features.ModuleRenewalPayment.Commands.CreateModuleRenewalPayment
{
    public sealed class CreateModuleRenewalPaymentCommand : IRequest<CreateModuleRenewalPaymentResponse>
    {
        public string TenantId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string ModuleCode { get; set; } = string.Empty;

        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
    }

    public sealed class CreateModuleRenewalPaymentResponse
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
