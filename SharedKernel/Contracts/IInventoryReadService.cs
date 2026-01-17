namespace SharedKernel.Contracts
{
    /// <summary>
    /// Read-only service interface for accessing Inventory module data.
    /// Implemented by Inventory module, consumed by Website module.
    /// This abstraction maintains module isolation.
    /// </summary>
    public interface IInventoryReadService
    {
        /// <summary>
        /// Get a single product by ID with full details.
        /// </summary>
        Task<InventoryProductDto?> GetProductByIdAsync(Guid inventoryProductId);

        /// <summary>
        /// Search products with pagination, sorting, and filtering.
        /// </summary>
        Task<InventoryProductSearchResult> SearchProductsAsync(InventoryProductSearchRequest request);

        /// <summary>
        /// Get a category by ID.
        /// </summary>
        Task<InventoryCategoryDto?> GetCategoryByIdAsync(Guid inventoryCategoryId);

        /// <summary>
        /// Search categories with pagination, sorting, and filtering.
        /// </summary>
        Task<InventoryCategorySearchResult> SearchCategoriesAsync(InventoryCategorySearchRequest request);
    }

    #region Product DTOs

    /// <summary>
    /// Search request for inventory products.
    /// </summary>
    public class InventoryProductSearchRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortBy { get; set; } // Name, CreatedAt, Price
        public string? SortDirection { get; set; } // Asc, Desc
        public Guid? CategoryId { get; set; }
        public bool? IsActive { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public string? SearchTerm { get; set; } // name, SKU, barcode
    }

    /// <summary>
    /// Search result for inventory products.
    /// </summary>
    public class InventoryProductSearchResult
    {
        public List<InventoryProductDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;
    }

    /// <summary>
    /// DTO for inventory product data shared across modules.
    /// </summary>
    public class InventoryProductDto
    {
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
        public string? MainImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    #endregion

    #region Category DTOs

    /// <summary>
    /// Search request for inventory categories.
    /// </summary>
    public class InventoryCategorySearchRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortBy { get; set; } // Name, CreatedAt
        public string? SortDirection { get; set; } // Asc, Desc
        public Guid? ParentCategoryId { get; set; }
        public string? SearchTerm { get; set; } // name
    }

    /// <summary>
    /// Search result for inventory categories.
    /// </summary>
    public class InventoryCategorySearchResult
    {
        public List<InventoryCategoryDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;
    }

    /// <summary>
    /// DTO for inventory category data shared across modules.
    /// </summary>
    public class InventoryCategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
        public bool IsActive { get; set; }
        public int ProductCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    #endregion
}

