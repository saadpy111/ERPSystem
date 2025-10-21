using MediatR;

namespace Inventory.Application.Features.ProductCostHistoryFeatures.Queries.GetProductCostHistories
{
    public class GetProductCostHistoriesQueryRequest : IRequest<GetProductCostHistoriesQueryResponse>
    {
        public Guid? ProductId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}