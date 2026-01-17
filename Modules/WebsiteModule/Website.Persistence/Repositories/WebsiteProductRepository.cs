using Microsoft.EntityFrameworkCore;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Persistence.Context;

namespace Website.Persistence.Repositories
{
    /// <summary>
    /// Website product repository implementation.
    /// </summary>
    public class WebsiteProductRepository : GenericRepository<WebsiteProduct>, IWebsiteProductRepository
    {
        public WebsiteProductRepository(WebsiteDbContext context) : base(context)
        {
        }

        public async Task<WebsiteProduct?> GetProductWithCategoryAsync(Guid id)
        {
            return await _dbSet
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<WebsiteProduct?> GetByInventoryProductIdAsync(Guid inventoryProductId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.InventoryProductId == inventoryProductId);
        }

        public async Task<List<WebsiteProduct>> GetPublishedProductsAsync(
            Guid? categoryId = null,
            Guid? collectionId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? search = null)
        {
            var query = _dbSet
                .Where(p => p.IsPublished && p.IsAvailable)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (collectionId.HasValue)
            {
                query = query.Where(p => p.CollectionItems.Any(ci => ci.CollectionId == collectionId.Value));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.NameSnapshot.Contains(search));
            }

            return await query
                .OrderBy(p => p.DisplayOrder)
                .ThenBy(p => p.NameSnapshot)
                .ToListAsync();
        }
    }
}
