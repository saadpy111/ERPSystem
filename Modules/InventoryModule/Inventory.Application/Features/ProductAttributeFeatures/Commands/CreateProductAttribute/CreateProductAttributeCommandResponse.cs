using Inventory.Application.Dtos.ProductAttributeDtos;

namespace Inventory.Application.Features.ProductAttributeFeatures.Commands.CreateProductAttribute
{
    public class CreateProductAttributeCommandResponse
    {
        public bool Success { get; set; }
        public GetProductAttributeDto? ProductAttribute { get; set; }
    }
}