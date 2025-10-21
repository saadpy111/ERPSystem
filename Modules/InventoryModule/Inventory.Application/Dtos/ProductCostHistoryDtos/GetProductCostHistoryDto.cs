namespace Inventory.Application.Dtos.ProductCostHistoryDtos
{
    public class GetProductCostHistoryDto
    {
        public Guid Id { get; set; }
        public decimal OldCost { get; set; }
        public decimal NewCost { get; set; }
        public Guid ProductId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}