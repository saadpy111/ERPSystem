using Inventory.Application.Pagination;
using Inventory.Application.Dtos.StockMoveDtos;

namespace Inventory.Application.Features.StockMoveFeatures.Queries.GetPagedStockMoves
{
    public class GetPagedStockMovesQueryResponse
    {
        public PagedResult<GetStockMoveDto> Result { get; set; }
    }
}