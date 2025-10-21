namespace Inventory.Application.Dtos.LocationDtos
{
    public class GetLocationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid? ParentId { get; set; }
        public List<GetLocationDto>? ChildLocations { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}