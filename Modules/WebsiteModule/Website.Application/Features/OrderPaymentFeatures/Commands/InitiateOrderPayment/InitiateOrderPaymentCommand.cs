using MediatR;

namespace Website.Application.Features.OrderPaymentFeatures.Commands.InitiateOrderPayment
{
    public class InitiateOrderPaymentCommand : IRequest<InitiateOrderPaymentResponse>
    {
        public Guid OrderId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }

    public class InitiateOrderPaymentResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? PaymentId { get; set; }
        public string? CheckoutUrl { get; set; }
        public string? ClientSecret { get; set; }
        public string? PublicKey { get; set; }
        public bool IsReused { get; set; }
    }
}
