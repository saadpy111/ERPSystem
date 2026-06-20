using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;

namespace Website.Application.Features.FavoriteFeatures.Queries.CheckFavoriteStatus
{
    public class CheckFavoriteStatusQueryHandler : IRequestHandler<CheckFavoriteStatusQuery, FavoriteStatusDto>
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public CheckFavoriteStatusQueryHandler(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<FavoriteStatusDto> Handle(CheckFavoriteStatusQuery request, CancellationToken cancellationToken)
        {
            var isFavorite = await _favoriteRepository.IsFavoriteAsync(request.UserId, request.ProductId, cancellationToken);

            return new FavoriteStatusDto
            {
                ProductId = request.ProductId,
                IsFavorite = isFavorite
            };
        }
    }
}
