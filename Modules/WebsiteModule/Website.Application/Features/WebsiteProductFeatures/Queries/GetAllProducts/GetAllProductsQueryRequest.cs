using MediatR;

namespace Website.Application.Features.WebsiteProductFeatures.Queries.GetAllProducts
{
    public class GetAllProductsQueryRequest : IRequest<GetAllProductsQueryResponse>
    {
        public Guid? CategoryId { get; set; }
        public Guid? CollectionId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public bool? IsPublished { get; set; }
    }
}
