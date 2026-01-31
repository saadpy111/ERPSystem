namespace Website.Application.Features.WebsiteProductFeatures.Queries.GetProductById
{
    public class GetProductByIdQueryResponse
    {
        public ProductDetailDto? Product { get; set; }
    }

    public class ProductDetailDto
    {
        public Guid Id { get; set; }
        public Guid InventoryProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<WebsiteProductImageDto> Images { get; set; } = new();
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsPublished { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class WebsiteProductImageDto
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? AltText { get; set; }
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
    }
}
