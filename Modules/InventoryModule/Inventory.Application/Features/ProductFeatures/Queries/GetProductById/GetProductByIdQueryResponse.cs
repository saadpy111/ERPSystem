using Inventory.Application.Dtos.ProductDtos;

namespace Inventory.Application.Features.ProductFeatures.Queries.GetProductById
{
    public class GetProductByIdQueryResponse
    {
        public GetProductDto? Product { get; set; }
    }
}