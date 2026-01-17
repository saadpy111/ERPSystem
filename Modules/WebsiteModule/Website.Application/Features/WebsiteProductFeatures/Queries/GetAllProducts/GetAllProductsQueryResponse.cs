using Website.Application.Pagination;

namespace Website.Application.Features.WebsiteProductFeatures.Queries.GetAllProducts
{
    public class GetAllProductsQueryResponse
    {
        public PagedResult<ProductListDto>? Result { get; set; }
    }

    public class ProductListDto
    {
        public Guid Id { get; set; }
        public Guid InventoryProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsPublished { get; set; }
        public int DisplayOrder { get; set; }
    }
}
