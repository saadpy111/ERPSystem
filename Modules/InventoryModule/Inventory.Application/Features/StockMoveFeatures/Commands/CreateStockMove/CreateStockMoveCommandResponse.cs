using Inventory.Application.Dtos.StockMoveDtos;

namespace Inventory.Application.Features.StockMoveFeatures.Commands.CreateStockMove
{
    public class CreateStockMoveCommandResponse
    {
        public bool Success { get; set; }
        public GetStockMoveDto? StockMove { get; set; }
    }
}