using Inventory.Application.Dtos.ProductDtos;

namespace Inventory.Application.Features.ProductFeatures.Commands.UpdateProduct
{
    public class UpdateProductCommandResponse
    {
        public bool Success { get; set; }
        public GetProductDto? Product { get; set; }
    }
}