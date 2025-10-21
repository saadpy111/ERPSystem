using Inventory.Application.Dtos.ProductDtos;
using MediatR;

namespace Inventory.Application.Features.ProductFeatures.Commands.UpdateProduct
{
    public class UpdateProductCommandRequest : IRequest<UpdateProductCommandResponse>
    {
        public UpdateProductDto Product { get; set; }
    }
}