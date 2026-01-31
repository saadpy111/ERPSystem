namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontProductById
{
    public class GetStorefrontProductByIdQueryResponse
    {
        public StorefrontProductDetailDto? Product { get; set; }
    }

    public class StorefrontProductDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<StorefrontProductImageDto> Images { get; set; } = new();
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class StorefrontProductImageDto
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? AltText { get; set; }
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
    }
}
