using MediatR;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontCategories
{
    public class GetStorefrontCategoriesQueryRequest : IRequest<GetStorefrontCategoriesQueryResponse>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
    }
}
