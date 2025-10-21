using Inventory.Application.Dtos.StockAdjustmentDtos;

namespace Inventory.Application.Features.StockAdjustmentFeatures.Queries.GetStockAdjustmentById
{
    public class GetStockAdjustmentByIdQueryResponse
    {
        public GetStockAdjustmentDto? StockAdjustment { get; set; }
    }
}