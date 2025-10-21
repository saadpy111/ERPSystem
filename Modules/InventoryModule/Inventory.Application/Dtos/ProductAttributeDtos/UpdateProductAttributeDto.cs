namespace Inventory.Application.Dtos.ProductAttributeDtos
{
    public class UpdateProductAttributeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
    }
}