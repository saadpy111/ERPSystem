namespace Website.Application.Features.WebsiteCategoryFeatures.Queries.GetInventoryCategories
{
    public class GetInventoryCategoriesQueryResponse
    {
        public List<InventoryCategoryListItem> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;
    }

    public class InventoryCategoryListItem
    {
        // All Inventory category data
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
        public bool IsActive { get; set; }
        public int ProductCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? ImagePath { get; set; }


        // Website-specific
        public bool IsAlreadyPublished { get; set; }
        public Guid? WebsiteCategoryId { get; set; }
    }
}
