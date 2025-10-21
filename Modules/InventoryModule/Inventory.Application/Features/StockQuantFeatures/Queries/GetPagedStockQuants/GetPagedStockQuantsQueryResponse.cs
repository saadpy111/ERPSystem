using Inventory.Application.Pagination;
using Inventory.Application.Dtos.StockQuantDtos;

namespace Inventory.Application.Features.StockQuantFeatures.Queries.GetPagedStockQuants
{
    public class GetPagedStockQuantsQueryResponse
    {
        public PagedResult<GetStockQuantDto> Result { get; set; }
    }
}