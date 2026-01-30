using MediatR;
using Website.Application.Pagination;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontCollections
{
    public class GetStorefrontCollectionsQueryRequest : IRequest<GetStorefrontCollectionsQueryResponse>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
    }
}
