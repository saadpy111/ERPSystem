using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;

namespace Website.Application.Features.FavoriteFeatures.Commands.RemoveFavorite
{
    public class RemoveFavoriteCommandHandler : IRequestHandler<RemoveFavoriteCommand, RemoveFavoriteResponse>
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public RemoveFavoriteCommandHandler(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<RemoveFavoriteResponse> Handle(RemoveFavoriteCommand request, CancellationToken cancellationToken)
        {
            var favorite = await _favoriteRepository.GetByUserAndProductAsync(request.UserId, request.ProductId, cancellationToken);
            if (favorite != null)
            {
                await _favoriteRepository.RemoveAsync(favorite);
                await _favoriteRepository.SaveChangesAsync(cancellationToken);
            }

            return new RemoveFavoriteResponse
            {
                Success = true,
                Message = "Product removed from favorites."
            };
        }
    }
}
