namespace Website.Application.Features.WebsiteProductFeatures.Queries.GetProductById
{
    public class GetProductByIdQueryResponse
    {
        public ProductDetailDto? Product { get; set; }
    }

    /// <summary>
    /// Detailed product data returned by the Website module storefront.
    /// Attributes are sourced read-only from the Inventory module via IInventoryReadService.
    /// </summary>
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

        /// <summary>
        /// Flat list of product attributes fetched read-only from the Inventory module.
        /// </summary>
        public List<ProductAttributeDto> Attributes { get; set; } = new();
    }

    /// <summary>
    /// A single product attribute key-value pair as displayed on the storefront.
    /// Mapped from Inventory.ProductAttributeValue via the SharedKernel contract.
    /// </summary>
    public class ProductAttributeDto
    {
        public string AttributeName { get; set; } = string.Empty;
        public string AttributeValue { get; set; } = string.Empty;
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
