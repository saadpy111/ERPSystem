namespace Inventory.Application.Dtos.ProductDtos
{
    public class GetProductStockLocationDto
    {
        public Guid LocationId { get; set; }
        public string LocationName { get; set; }
        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int Quantity { get; set; }
        public int ReservedQuantity { get; set; }
    }
}