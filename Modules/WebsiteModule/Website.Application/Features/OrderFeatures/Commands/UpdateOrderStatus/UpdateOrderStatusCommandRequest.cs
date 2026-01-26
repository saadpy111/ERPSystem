using MediatR;
using Website.Domain.Enums;

namespace Website.Application.Features.OrderFeatures.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandRequest : IRequest<UpdateOrderStatusCommandResponse>
    {
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
