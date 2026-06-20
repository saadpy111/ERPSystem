using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;

namespace Website.Application.Features.FavoriteFeatures.Queries.GetFavoritesCount
{
    public class GetFavoritesCountQueryHandler : IRequestHandler<GetFavoritesCountQuery, FavoriteCountDto>
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public GetFavoritesCountQueryHandler(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<FavoriteCountDto> Handle(GetFavoritesCountQuery request, CancellationToken cancellationToken)
        {
            var count = await _favoriteRepository.GetFavoritesCountAsync(request.UserId, cancellationToken);

            return new FavoriteCountDto
            {
                Count = count
            };
        }
    }
}
