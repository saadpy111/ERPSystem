namespace Inventory.Application.Dtos.WarehouseDtos
{
    public class UpdateWarehouseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Location { get; set; }
    }
}