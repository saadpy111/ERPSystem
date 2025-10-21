using Inventory.Application.Dtos.ProductCostHistoryDtos;

namespace Inventory.Application.Features.ProductCostHistoryFeatures.Queries.GetProductCostHistories
{
    public class GetProductCostHistoriesQueryResponse
    {
        public List<GetProductCostHistoryDto> CostHistories { get; set; } = new();
    }
}