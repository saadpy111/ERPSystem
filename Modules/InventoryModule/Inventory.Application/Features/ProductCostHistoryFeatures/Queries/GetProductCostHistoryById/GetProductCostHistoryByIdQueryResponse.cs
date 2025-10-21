using Inventory.Application.Dtos.ProductCostHistoryDtos;

namespace Inventory.Application.Features.ProductCostHistoryFeatures.Queries.GetProductCostHistoryById
{
    public class GetProductCostHistoryByIdQueryResponse
    {
        public GetProductCostHistoryDto? ProductCostHistory { get; set; }
    }
}