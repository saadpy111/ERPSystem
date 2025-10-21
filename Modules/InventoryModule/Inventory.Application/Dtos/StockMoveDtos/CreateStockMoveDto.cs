using Inventory.Domain.Enums;

namespace Inventory.Application.Dtos.StockMoveDtos
{
    public class CreateStockMoveDto
    {
        public int Quantity { get; set; }
        public string Reference { get; set; }
        public StockMoveType MoveType { get; set; }
        public Guid ProductId { get; set; }
        public Guid SourceLocationId { get; set; }
        public Guid DestinationLocationId { get; set; }
    }
}