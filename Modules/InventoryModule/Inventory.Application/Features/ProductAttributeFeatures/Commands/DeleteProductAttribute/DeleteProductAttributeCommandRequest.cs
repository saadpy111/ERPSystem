using MediatR;

namespace Inventory.Application.Features.ProductAttributeFeatures.Commands.DeleteProductAttribute
{
    public class DeleteProductAttributeCommandRequest : IRequest<DeleteProductAttributeCommandResponse>
    {
        public Guid Id { get; set; }
    }
}