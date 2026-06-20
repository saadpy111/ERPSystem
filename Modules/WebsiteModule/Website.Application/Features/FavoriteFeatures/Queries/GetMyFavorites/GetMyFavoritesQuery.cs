using MediatR;
using Website.Application.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Features.FavoriteFeatures.Queries.GetMyFavorites
{
    public class GetMyFavoritesQuery : IRequest<PagedResult<FavoriteProductDto>>
    {
        public string UserId { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
    }
}
