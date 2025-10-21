namespace Inventory.Application.Dtos.StockAdjustmentDtos
{
    public class GetStockAdjustmentDto
    {
        public Guid Id { get; set; }
        public int ExpectedQuantity { get; set; }
        public int ActualQuantity { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid ProductId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}