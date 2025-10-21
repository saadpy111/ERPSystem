namespace Inventory.Application.Dtos.WarehouseDtos
{
    public class GetWarehouseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Location { get; set; }
    }
}