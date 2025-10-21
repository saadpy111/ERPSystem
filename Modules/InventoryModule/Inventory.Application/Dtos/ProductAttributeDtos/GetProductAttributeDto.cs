namespace Inventory.Application.Dtos.ProductAttributeDtos
{
    public class GetProductAttributeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}