namespace Website.Application.Features.CollectionFeatures.Queries.GetCollectionById
{
    public class GetCollectionByIdQueryResponse
    {
        public CollectionDetailDto? Collection { get; set; }
    }

    public class CollectionDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public List<CollectionProductDto> Products { get; set; } = new();
    }

    public class CollectionProductDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
