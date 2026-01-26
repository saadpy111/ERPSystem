using MediatR;
using Website.Domain.Enums;

namespace Website.Application.Features.OrderFeatures.Queries.GetAllOrders
{
    public class GetAllOrdersQueryRequest : IRequest<GetAllOrdersQueryResponse>
    {
        public OrderStatus? Status { get; set; }
    }
}
