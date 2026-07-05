using MediatR;using SharedKernel.Enums;

namespace Subscription.Application.Features.CompanyPayment.Commands.CreateCompanyPayment
{
    public sealed class CreateCompanyPaymentCommand : IRequest<CreateCompanyPaymentResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyCode { get; set; } = string.Empty;
        public string PlanCode { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = "EGP";
        public BillingInterval Interval { get; set; }

        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
    }

    public sealed class CreateCompanyPaymentResponse
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
