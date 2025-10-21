using Inventory.Application.Dtos.ProductDtos;

namespace Inventory.Application.Features.ProductFeatures.Commands.CreateProduct
{
    public class CreateProductCommandResponse
    {
        public bool Success { get; set; }
        public GetProductDto? Product { get; set; }
    }
}