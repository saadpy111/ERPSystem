using Inventory.Application.Pagination;
using Inventory.Application.Dtos.ProductCostHistoryDtos;

namespace Inventory.Application.Features.ProductCostHistoryFeatures.Queries.GetPagedProductCostHistories
{
    public class GetPagedProductCostHistoriesQueryResponse
    {
        public PagedResult<GetProductCostHistoryDto> Result { get; set; }
    }
}