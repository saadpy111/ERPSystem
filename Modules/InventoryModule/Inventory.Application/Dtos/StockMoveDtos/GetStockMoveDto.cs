using Inventory.Domain.Enums;

namespace Inventory.Application.Dtos.StockMoveDtos
{
    public class GetStockMoveDto
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public DateTime MoveDate { get; set; }
        public string Reference { get; set; }
        public StockMoveType MoveType { get; set; }
        public Guid ProductId { get; set; }
        public Guid SourceLocationId { get; set; }
        public Guid DestinationLocationId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}