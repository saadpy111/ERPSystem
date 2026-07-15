using MediatR;

namespace Website.Application.Features.OrderPaymentFeatures.Queries.GetOrderPaymentStatus
{
    public class GetOrderPaymentStatusQuery : IRequest<GetOrderPaymentStatusResponse>
    {
        public Guid OrderId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }

    public class GetOrderPaymentStatusResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public bool IsPaid { get; set; }
    }
}
