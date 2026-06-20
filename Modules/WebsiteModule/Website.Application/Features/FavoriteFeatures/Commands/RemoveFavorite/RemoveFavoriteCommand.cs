using MediatR;
using Website.Application.DTOs;

namespace Website.Application.Features.FavoriteFeatures.Commands.RemoveFavorite
{
    public class RemoveFavoriteCommand : IRequest<RemoveFavoriteResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
    }
}
