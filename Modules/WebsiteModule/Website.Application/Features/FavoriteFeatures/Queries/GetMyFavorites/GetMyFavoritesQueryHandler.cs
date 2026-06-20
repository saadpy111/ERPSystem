using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Features.FavoriteFeatures.Queries.GetMyFavorites
{
    public class GetMyFavoritesQueryHandler : IRequestHandler<GetMyFavoritesQuery, PagedResult<FavoriteProductDto>>
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public GetMyFavoritesQueryHandler(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<PagedResult<FavoriteProductDto>> Handle(GetMyFavoritesQuery request, CancellationToken cancellationToken)
        {
            return await _favoriteRepository.GetUserFavoritesPagedAsync(
                request.UserId,
                request.PageNumber,
                request.PageSize,
                request.SearchTerm,
                cancellationToken);
        }
    }
}
