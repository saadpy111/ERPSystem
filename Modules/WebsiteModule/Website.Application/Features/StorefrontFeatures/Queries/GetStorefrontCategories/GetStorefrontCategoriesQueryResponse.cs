namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontCategories
{
    public class GetStorefrontCategoriesQueryResponse
    {
        public List<StorefrontCategoryDto> Categories { get; set; } = new();
    }

    public class StorefrontCategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public Guid? ParentCategoryId { get; set; }
        public int ProductCount { get; set; }
        public List<StorefrontCategoryDto> Children { get; set; } = new();
    }
}
