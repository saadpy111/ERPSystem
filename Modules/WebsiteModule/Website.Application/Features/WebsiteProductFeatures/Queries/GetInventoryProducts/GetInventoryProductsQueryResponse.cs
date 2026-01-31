using SharedKernel.Contracts;

namespace Website.Application.Features.WebsiteProductFeatures.Queries.GetInventoryProducts
{
    public class GetInventoryProductsQueryResponse
    {
        public List<InventoryProductListItem> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public int TotalPages =>
            PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;
    }

    public class InventoryProductListItem
    {
        // ===== Inventory Product =====
        public Guid Id { get; set; }
        public string? Sku { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? UnitOfMeasure { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public bool IsActive { get; set; }
        public string? ProductBarcode { get; set; }
        public string? MainSupplierName { get; set; }
        public decimal? Tax { get; set; }
        public int? OrderLimit { get; set; }
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }

        // Images
        public string? MainImageUrl { get; set; }
        public List<InventoryProductImageDto> Images { get; set; } = new();

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // ===== Website =====
        public bool IsAlreadyPublished { get; set; }
        public Guid? WebsiteProductId { get; set; }
    }

    public class InventoryProductImageDto
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; } = null!;
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
    }
}
