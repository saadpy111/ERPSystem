namespace Inventory.Application.Dtos.StockQuantDtos
{
    public class UpdateStockQuantDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid LocationId { get; set; }
        public int Quantity { get; set; }
    }
}