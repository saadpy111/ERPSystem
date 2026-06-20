using Website.Application.DTOs;
using Website.Application.Pagination;
using Website.Domain.Entities;

namespace Website.Application.Contracts.Persistence.Repositories
{
    public interface IFavoriteRepository
    {
        Task<FavoriteProduct?> GetByUserAndProductAsync(string userId, Guid productId, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(string userId, Guid productId, CancellationToken cancellationToken = default);
        Task<PagedResult<FavoriteProductDto>> GetUserFavoritesPagedAsync(string userId, int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken = default);
        Task<bool> IsFavoriteAsync(string userId, Guid productId, CancellationToken cancellationToken = default);
        Task<int> GetFavoritesCountAsync(string userId, CancellationToken cancellationToken = default);
        Task AddAsync(FavoriteProduct favorite);
        Task RemoveAsync(FavoriteProduct favorite);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
