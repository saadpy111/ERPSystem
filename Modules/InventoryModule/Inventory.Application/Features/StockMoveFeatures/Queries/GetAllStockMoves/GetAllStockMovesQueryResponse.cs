using Inventory.Application.Dtos.StockMoveDtos;
using System.Collections.Generic;

namespace Inventory.Application.Features.StockMoveFeatures.Queries.GetAllStockMoves
{
    public class GetAllStockMovesQueryResponse
    {
        public List<GetStockMoveDto> StockMoves { get; set; } = new();
    }
}