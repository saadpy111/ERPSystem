using Website.Domain.Entities;

namespace Website.Application.Contracts.Persistence.Repositories
{
    /// <summary>
    /// Website product-specific repository interface.
    /// </summary>
    public interface IWebsiteProductRepository : IGenericRepository<WebsiteProduct>
    {
        /// <summary>
        /// Get product with category loaded.
        /// </summary>
        Task<WebsiteProduct?> GetProductWithCategoryAsync(Guid id);

        /// <summary>
        /// Get product by inventory product ID.
        /// </summary>
        Task<WebsiteProduct?> GetByInventoryProductIdAsync(Guid inventoryProductId);

        /// <summary>
        /// Get published products with filters.
        /// </summary>
        Task<List<WebsiteProduct>> GetPublishedProductsAsync(
            Guid? categoryId = null,
            Guid? collectionId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? search = null);
    }
}
