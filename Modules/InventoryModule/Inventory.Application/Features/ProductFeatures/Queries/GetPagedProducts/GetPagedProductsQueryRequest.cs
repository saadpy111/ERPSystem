using MediatR;

namespace Inventory.Application.Features.ProductFeatures.Queries.GetPagedProducts
{
    public class GetPagedProductsQueryRequest : IRequest<GetPagedProductsQueryResponse>
    {
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}