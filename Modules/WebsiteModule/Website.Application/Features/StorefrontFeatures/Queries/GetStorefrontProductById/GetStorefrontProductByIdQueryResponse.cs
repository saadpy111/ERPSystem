namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontProductById
{
    public class GetStorefrontProductByIdQueryResponse
    {
        public StorefrontProductDetailDto? Product { get; set; }
    }

    /// <summary>
    /// Full product detail as exposed on the public storefront.
    /// Attributes are sourced read-only from the Inventory module via IInventoryReadService.
    /// </summary>
    public class StorefrontProductDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<StorefrontProductImageDto> Images { get; set; } = new();
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }

        /// <summary>
        /// Flat list of product attributes fetched read-only from the Inventory module.
        /// </summary>
        public List<StorefrontProductAttributeDto> Attributes { get; set; } = new();
    }

    /// <summary>
    /// A single attribute key-value pair for storefront display.
    /// Mapped from Inventory.ProductAttributeValue via the SharedKernel contract.
    /// </summary>
    public class StorefrontProductAttributeDto
    {
        public string AttributeName  { get; set; } = string.Empty;
        public string AttributeValue { get; set; } = string.Empty;
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
