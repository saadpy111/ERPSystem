using MediatR;

namespace Website.Application.Features.OrderFeatures.Queries.GetUserOrders
{
    public class GetUserOrdersQueryRequest : IRequest<GetUserOrdersQueryResponse>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
