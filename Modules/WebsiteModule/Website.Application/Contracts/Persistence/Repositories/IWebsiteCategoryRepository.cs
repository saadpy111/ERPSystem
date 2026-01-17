using Website.Domain.Entities;

namespace Website.Application.Contracts.Persistence.Repositories
{
    /// <summary>
    /// Website category-specific repository interface.
    /// </summary>
    public interface IWebsiteCategoryRepository : IGenericRepository<WebsiteCategory>
    {
        /// <summary>
        /// Get category with children loaded.
        /// </summary>
        Task<WebsiteCategory?> GetCategoryWithChildrenAsync(Guid id);

        /// <summary>
        /// Get category by inventory category ID.
        /// </summary>
        Task<WebsiteCategory?> GetByInventoryCategoryIdAsync(Guid inventoryCategoryId);

        /// <summary>
        /// Get root categories (no parent).
        /// </summary>
        Task<List<WebsiteCategory>> GetRootCategoriesAsync();

        /// <summary>
        /// Get all active categories.
        /// </summary>
        Task<List<WebsiteCategory>> GetActiveCategoriesAsync();
    }
}
