using MediatR;
using Website.Application.DTOs;

namespace Website.Application.Features.FavoriteFeatures.Commands.AddFavorite
{
    public class AddFavoriteCommand : IRequest<AddFavoriteResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
    }
}
