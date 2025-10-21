using MediatR;

namespace Inventory.Application.Features.ProductCostHistoryFeatures.Queries.GetProductCostHistoryById
{
    public class GetProductCostHistoryByIdQueryRequest : IRequest<GetProductCostHistoryByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}