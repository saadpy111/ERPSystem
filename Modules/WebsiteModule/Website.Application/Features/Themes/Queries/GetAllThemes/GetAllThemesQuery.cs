using MediatR;
using Website.Application.DTOs;

namespace Website.Application.Features.Themes.Queries.GetAllThemes
{
    public class GetAllThemesQuery : IRequest<GetAllThemesResponse>
    {
        public bool ActiveOnly { get; set; } = true;
    }

    public class GetAllThemesResponse
    {
        public bool Success { get; set; }
        public List<ThemeDto> Themes { get; set; } = new();
    }
}
