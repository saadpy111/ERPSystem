namespace Website.Application.Features.CollectionFeatures.Queries.GetAllCollections
{
    public class GetAllCollectionsQueryResponse
    {
        public List<CollectionDto> Collections { get; set; } = new();
    }

    public class CollectionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public int ProductCount { get; set; }
    }
}
