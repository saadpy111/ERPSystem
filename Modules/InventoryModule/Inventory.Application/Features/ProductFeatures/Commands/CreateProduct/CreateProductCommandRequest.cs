using Inventory.Application.Dtos.ProductDtos;
using MediatR;

namespace Inventory.Application.Features.ProductFeatures.Commands.CreateProduct
{
    public class CreateProductCommandRequest : IRequest<CreateProductCommandResponse>
    {
        public CreateProductDto Product { get; set; }
    }
}