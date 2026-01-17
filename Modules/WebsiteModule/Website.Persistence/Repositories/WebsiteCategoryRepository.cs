using Microsoft.EntityFrameworkCore;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Persistence.Context;

namespace Website.Persistence.Repositories
{
    /// <summary>
    /// Website category repository implementation.
    /// </summary>
    public class WebsiteCategoryRepository : GenericRepository<WebsiteCategory>, IWebsiteCategoryRepository
    {
        public WebsiteCategoryRepository(WebsiteDbContext context) : base(context)
        {
        }

        public async Task<WebsiteCategory?> GetCategoryWithChildrenAsync(Guid id)
        {
            return await _dbSet
                .Include(c => c.ChildCategories)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<WebsiteCategory?> GetByInventoryCategoryIdAsync(Guid inventoryCategoryId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.InventoryCategoryId == inventoryCategoryId);
        }

        public async Task<List<WebsiteCategory>> GetRootCategoriesAsync()
        {
            return await _dbSet
                .Where(c => c.ParentCategoryId == null && c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
        }

        public async Task<List<WebsiteCategory>> GetActiveCategoriesAsync()
        {
            return await _dbSet
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
        }
    }
}
