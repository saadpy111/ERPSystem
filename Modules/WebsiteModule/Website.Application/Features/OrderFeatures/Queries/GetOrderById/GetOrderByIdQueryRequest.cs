using MediatR;

namespace Website.Application.Features.OrderFeatures.Queries.GetOrderById
{
    public class GetOrderByIdQueryRequest : IRequest<GetOrderByIdQueryResponse>
    {
        public Guid OrderId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
