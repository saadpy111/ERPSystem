using Inventory.Application.Pagination;
using Inventory.Application.Dtos.StockAdjustmentDtos;

namespace Inventory.Application.Features.StockAdjustmentFeatures.Queries.GetPagedStockAdjustments
{
    public class GetPagedStockAdjustmentsQueryResponse
    {
        public PagedResult<GetStockAdjustmentDto> Result { get; set; }
    }
}