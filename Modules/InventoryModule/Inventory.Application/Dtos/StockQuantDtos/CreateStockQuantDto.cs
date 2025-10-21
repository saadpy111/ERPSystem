namespace Inventory.Application.Dtos.StockQuantDtos
{
    public class CreateStockQuantDto
    {
        public Guid ProductId { get; set; }
        public Guid LocationId { get; set; }
        public int Quantity { get; set; }
    }
}