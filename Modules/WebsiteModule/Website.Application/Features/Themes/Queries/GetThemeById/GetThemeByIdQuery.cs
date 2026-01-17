using MediatR;
using Website.Application.DTOs;

namespace Website.Application.Features.Themes.Queries.GetThemeById
{
    public class GetThemeByIdQuery : IRequest<GetThemeByIdResponse>
    {
        public Guid ThemeId { get; set; }
    }

    public class GetThemeByIdResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public ThemeDto? Theme { get; set; }
    }
}
