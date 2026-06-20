using MediatR;
using Website.Application.DTOs;

namespace Website.Application.Features.FavoriteFeatures.Queries.CheckFavoriteStatus
{
    public class CheckFavoriteStatusQuery : IRequest<FavoriteStatusDto>
    {
        public string UserId { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
    }
}
