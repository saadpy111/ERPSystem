using Inventory.Application.Dtos.StockMoveDtos;

namespace Inventory.Application.Features.StockMoveFeatures.Queries.GetStockMoveById
{
    public class GetStockMoveByIdQueryResponse
    {
        public GetStockMoveDto? StockMove { get; set; }
    }
}