using Inventory.Application.Dtos.ProductAttributeDtos;
using MediatR;

namespace Inventory.Application.Features.ProductAttributeFeatures.Commands.CreateProductAttribute
{
    public class CreateProductAttributeCommandRequest : IRequest<CreateProductAttributeCommandResponse>
    {
        public CreateProductAttributeDto ProductAttribute { get; set; }
    }
}