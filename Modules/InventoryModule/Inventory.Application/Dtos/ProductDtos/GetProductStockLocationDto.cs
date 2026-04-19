namespace Inventory.Application.Dtos.ProductDtos
{
    public class GetProductStockLocationDto
    {
        public Guid LocationId { get; set; }
        public string LocationName { get; set; }
        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public decimal Quantity { get; set; }
        public decimal ReservedQuantity { get; set; }
    }
}