using MediatR;

namespace Website.Application.Features.Themes.Commands.DeleteTheme
{
    public class DeleteThemeCommand : IRequest<DeleteThemeResponse>
    {
        public Guid ThemeId { get; set; }
    }

    public class DeleteThemeResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
