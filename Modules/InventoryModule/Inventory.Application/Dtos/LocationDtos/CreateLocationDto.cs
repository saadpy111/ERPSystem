namespace Inventory.Application.Dtos.LocationDtos
{
    public class CreateLocationDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid? ParentId { get; set; }
    }
}