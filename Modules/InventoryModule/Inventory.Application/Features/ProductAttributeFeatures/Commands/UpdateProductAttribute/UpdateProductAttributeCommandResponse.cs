using Inventory.Application.Dtos.ProductAttributeDtos;

namespace Inventory.Application.Features.ProductAttributeFeatures.Commands.UpdateProductAttribute
{
    public class UpdateProductAttributeCommandResponse
    {
        public bool Success { get; set; }

        public GetProductAttributeDto? ProductAttribute { get; set; }
    }
}