using MediatR;
using Website.Application.DTOs;

namespace Website.Application.Features.OrderFeatures.Queries.GetUserOrderDetails
{
    public class GetUserOrderDetailsQuery : IRequest<UserOrderDetailsDto?>
    {
        public Guid OrderId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
