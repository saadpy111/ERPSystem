using MediatR;
using Website.Application.DTOs;

namespace Website.Application.Features.OrderFeatures.Queries.GetAdminOrderDetails
{
    public class GetAdminOrderDetailsQuery : IRequest<OrderDetailsDto?>
    {
        public Guid OrderId { get; set; }
    }
}
