namespace Website.Domain.Entities
{
    /// <summary>
    /// Website category for storefront navigation.
    /// Can differ from Inventory categories for better marketing presentation.
    /// </summary>
    public class WebsiteCategory : BaseEntity
    {
        /// <summary>
        /// Reference to the original category in Inventory Module.
        /// </summary>
        public Guid? InventoryCategoryId { get; set; }

        /// <summary>
        /// Display name for storefront navigation.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// URL-friendly identifier for SEO.
        /// </summary>
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Parent category for hierarchical navigation.
        /// </summary>
        public Guid? ParentCategoryId { get; set; }
        public WebsiteCategory? ParentCategory { get; set; }

        /// <summary>
        /// Display order for sorting categories.
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Whether the category is visible on storefront.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Relative path to category image.
        /// </summary>
        public string? ImagePath { get; set; }

        // Navigation properties
        public ICollection<WebsiteCategory> ChildCategories { get; set; } = new List<WebsiteCategory>();
        public ICollection<WebsiteProduct> Products { get; set; } = new List<WebsiteProduct>();
    }
}
