namespace Website.Domain.Entities
{
    /// <summary>
    /// Represents a product published on the website storefront.
    /// Contains snapshot data from the Inventory module - never edits Inventory directly.
    /// </summary>
    public class WebsiteProduct : BaseEntity
    {
        /// <summary>
        /// Reference to the original product in Inventory Module.
        /// </summary>
        public Guid InventoryProductId { get; set; }

        /// <summary>
        /// Snapshot of product name at time of publish.
        /// </summary>
        public string NameSnapshot { get; set; } = string.Empty;

        /// <summary>
        /// Snapshot of product image URL.
        /// </summary>
        public string? ImageUrlSnapshot { get; set; }

        /// <summary>
        /// Website category this product belongs to.
        /// </summary>
        public Guid CategoryId { get; set; }
        public WebsiteCategory? Category { get; set; }

        /// <summary>
        /// Snapshot of category name for quick display.
        /// </summary>
        public string CategoryNameSnapshot { get; set; } = string.Empty;

        /// <summary>
        /// Selling price set during publish. Updated via price change events.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Stock availability status. Updated via stock change events.
        /// </summary>
        public bool IsAvailable { get; set; } = true;

        /// <summary>
        /// Whether the product is visible on the storefront.
        /// </summary>
        public bool IsPublished { get; set; } = false;

        /// <summary>
        /// Display order for sorting products.
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        // Navigation properties
        public ICollection<ProductCollectionItem> CollectionItems { get; set; } = new List<ProductCollectionItem>();
        public ICollection<OfferProduct> OfferProducts { get; set; } = new List<OfferProduct>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
