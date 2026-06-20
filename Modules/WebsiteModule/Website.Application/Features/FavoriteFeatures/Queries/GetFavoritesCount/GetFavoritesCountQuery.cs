using MediatR;
using Website.Application.DTOs;

namespace Website.Application.Features.FavoriteFeatures.Queries.GetFavoritesCount
{
    public class GetFavoritesCountQuery : IRequest<FavoriteCountDto>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
