namespace Inventory.Application.Dtos.ProductDtos
{
    public class GetProductStockLocationDto
    {
        public string LocationName { get; set; }
        public string WarehouseName { get; set; }
        public int Quantity { get; set; }
        public int ReservedQuantity { get; set; }
    }
}
