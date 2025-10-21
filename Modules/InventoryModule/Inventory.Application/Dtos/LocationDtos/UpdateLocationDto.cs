namespace Inventory.Application.Dtos.LocationDtos
{
    public class UpdateLocationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid? ParentId { get; set; }
    }
}