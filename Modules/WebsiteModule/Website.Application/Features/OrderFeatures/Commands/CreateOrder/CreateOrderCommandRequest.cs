using MediatR;
using Website.Domain.Enums;

namespace Website.Application.Features.OrderFeatures.Commands.CreateOrder
{
    public class CreateOrderCommandRequest : IRequest<CreateOrderCommandResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public PaymentMethod PaymentMethod { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
