namespace Inventory.Application.Dtos.StockQuantDtos
{
    public class GetStockQuantDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid LocationId { get; set; }
        public int Quantity { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}