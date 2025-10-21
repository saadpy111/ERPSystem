using Inventory.Application.Pagination;
using Inventory.Application.Dtos.ProductDtos;

namespace Inventory.Application.Features.ProductFeatures.Queries.GetPagedProducts
{
    public class GetPagedProductsQueryResponse
    {
        public PagedResult<GetProductDto> Result { get; set; }
    }
}