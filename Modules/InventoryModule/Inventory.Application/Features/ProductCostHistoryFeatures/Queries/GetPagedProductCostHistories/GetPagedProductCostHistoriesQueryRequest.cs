using MediatR;

namespace Inventory.Application.Features.ProductCostHistoryFeatures.Queries.GetPagedProductCostHistories
{
    public class GetPagedProductCostHistoriesQueryRequest : IRequest<GetPagedProductCostHistoriesQueryResponse>
    {
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}