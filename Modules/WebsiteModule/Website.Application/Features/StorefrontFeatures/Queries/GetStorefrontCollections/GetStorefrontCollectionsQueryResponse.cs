using Website.Application.Pagination;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontCollections
{
    public class GetStorefrontCollectionsQueryResponse
    {
        public PagedResult<StorefrontCollectionDto>? Result { get; set; }
    }

    public class StorefrontCollectionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
