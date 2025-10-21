using Inventory.Application.Dtos.ProductAttributeDtos;
using MediatR;

namespace Inventory.Application.Features.ProductAttributeFeatures.Commands.UpdateProductAttribute
{
    public class UpdateProductAttributeCommandRequest : IRequest<UpdateProductAttributeCommandResponse>
    {
        public UpdateProductAttributeDto ProductAttribute { get; set; }
    }
}