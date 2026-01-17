using MediatR;
using SharedKernel.Contracts;

namespace Website.Application.Features.WebsiteProductFeatures.Queries.GetInventoryProducts
{
    public class GetInventoryProductsQueryRequest : IRequest<GetInventoryProductsQueryResponse>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortBy { get; set; } // Name, CreatedAt, Price
        public string? SortDirection { get; set; } // Asc, Desc
        public Guid? InventoryCategoryId { get; set; }
        public bool? IsActive { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public string? SearchTerm { get; set; }
    }
}
