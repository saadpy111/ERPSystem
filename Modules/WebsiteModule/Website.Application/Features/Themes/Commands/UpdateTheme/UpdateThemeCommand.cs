using MediatR;
using Website.Domain.ValueObjects;

namespace Website.Application.Features.Themes.Commands.UpdateTheme
{
    public class UpdateThemeCommand : IRequest<UpdateThemeResponse>
    {
        public Guid ThemeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PreviewImage { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public ThemeConfig Config { get; set; } = new();
    }

    public class UpdateThemeResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
