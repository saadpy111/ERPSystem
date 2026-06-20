using Microsoft.EntityFrameworkCore;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;
using Website.Application.Pagination;
using Website.Domain.Entities;
using Website.Persistence.Context;

namespace Website.Persistence.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly WebsiteDbContext _context;

        public FavoriteRepository(WebsiteDbContext context)
        {
            _context = context;
        }

        public async Task<FavoriteProduct?> GetByUserAndProductAsync(string userId, Guid productId, CancellationToken cancellationToken = default)
        {
            return await _context.FavoriteProducts
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId, cancellationToken);
        }

        public async Task<bool> ExistsAsync(string userId, Guid productId, CancellationToken cancellationToken = default)
        {
            return await _context.FavoriteProducts
                .AsNoTracking()
                .AnyAsync(f => f.UserId == userId && f.ProductId == productId, cancellationToken);
        }

        public async Task<PagedResult<FavoriteProductDto>> GetUserFavoritesPagedAsync(
            string userId, int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken = default)
        {
            var query = _context.FavoriteProducts
                .AsNoTracking()
                .Include(f => f.Product)
                    .ThenInclude(p => p.Images)
                .Where(f => f.UserId == userId && f.Product.IsPublished);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(f => f.Product.NameSnapshot.Contains(searchTerm)
                    || f.Product.CategoryNameSnapshot.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(f => f.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new FavoriteProductDto
                {
                    ProductId = f.ProductId,
                    Name = f.Product.NameSnapshot,
                    Description = null,
                    SKU = null,
                    Price = f.Product.Price,
                    DiscountPrice = null,
                    FinalPrice = null,
                    MainImageUrl = f.Product.Images
                        .Where(i => i.IsPrimary)
                        .Select(i => i.ImagePath)
                        .FirstOrDefault()
                        ?? f.Product.Images
                            .OrderBy(i => i.DisplayOrder)
                            .Select(i => i.ImagePath)
                            .FirstOrDefault(),
                    IsAvailable = f.Product.IsAvailable,
                    CategoryId = f.Product.CategoryId,
                    CategoryName = f.Product.CategoryNameSnapshot,
                    Rating = null,
                    ReviewCount = null,
                    AddedToFavoritesAt = f.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<FavoriteProductDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<bool> IsFavoriteAsync(string userId, Guid productId, CancellationToken cancellationToken = default)
        {
            return await _context.FavoriteProducts
                .AsNoTracking()
                .AnyAsync(f => f.UserId == userId && f.ProductId == productId && f.Product.IsPublished, cancellationToken);
        }

        public async Task<int> GetFavoritesCountAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _context.FavoriteProducts
                .AsNoTracking()
                .CountAsync(f => f.UserId == userId && f.Product.IsPublished, cancellationToken);
        }

        public async Task AddAsync(FavoriteProduct favorite)
        {
            await _context.FavoriteProducts.AddAsync(favorite);
        }

        public Task RemoveAsync(FavoriteProduct favorite)
        {
            _context.FavoriteProducts.Remove(favorite);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
