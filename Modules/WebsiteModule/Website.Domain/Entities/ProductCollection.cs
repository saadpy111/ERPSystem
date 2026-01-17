namespace Website.Domain.Entities
{
    /// <summary>
    /// Marketing collections for grouping products (e.g., "Summer Sale", "Best Sellers").
    /// A product can belong to multiple collections.
    /// </summary>
    public class ProductCollection : BaseEntity
    {
        /// <summary>
        /// Collection name for display.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// URL-friendly identifier.
        /// </summary>
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Marketing description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Banner or featured image for the collection.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Whether the collection is visible on storefront.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Display order for sorting collections.
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        // Navigation properties
        public ICollection<ProductCollectionItem> Items { get; set; } = new List<ProductCollectionItem>();
    }
}
