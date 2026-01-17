using Microsoft.EntityFrameworkCore;
using Website.Application.Contracts.Persistence;
using Website.Domain.Entities;
using Website.Persistence.Context;

namespace Website.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for Theme entity.
    /// </summary>
    public class ThemeRepository : IThemeRepository
    {
        private readonly WebsiteDbContext _context;

        public ThemeRepository(WebsiteDbContext context)
        {
            _context = context;
        }

        public async Task<Theme?> GetByIdAsync(Guid id)
        {
            return await _context.Themes.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Theme?> GetByCodeAsync(string code)
        {
            return await _context.Themes
                .FirstOrDefaultAsync(t => t.Code.ToLower() == code.ToLower());
        }

        public async Task<List<Theme>> GetAllAsync(bool activeOnly = true)
        {
            var query = _context.Themes.AsQueryable();
            
            if (activeOnly)
                query = query.Where(t => t.IsActive);
            
            return await query.OrderBy(t => t.Name).ToListAsync();
        }

        public async Task<Theme> CreateAsync(Theme theme)
        {
            theme.CreatedAt = DateTime.UtcNow;
            theme.UpdatedAt = DateTime.UtcNow;
            
            await _context.Themes.AddAsync(theme);
            return theme;
        }

        public async Task<Theme> UpdateAsync(Theme theme)
        {
            theme.UpdatedAt = DateTime.UtcNow;
            _context.Themes.Update(theme);
            return await Task.FromResult(theme);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var theme = await GetByIdAsync(id);
            if (theme == null) return false;
            
            _context.Themes.Remove(theme);
            return true;
        }

        public async Task<bool> ExistsAsync(string code)
        {
            return await _context.Themes.AnyAsync(t => t.Code.ToLower() == code.ToLower());
        }
    }
}
