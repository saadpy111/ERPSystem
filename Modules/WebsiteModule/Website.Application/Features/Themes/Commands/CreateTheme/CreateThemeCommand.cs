using MediatR;
using Website.Domain.ValueObjects;

namespace Website.Application.Features.Themes.Commands.CreateTheme
{
    public class CreateThemeCommand : IRequest<CreateThemeResponse>
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PreviewImage { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public ThemeConfig Config { get; set; } = new();
    }

    public class CreateThemeResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public Guid? ThemeId { get; set; }
    }
}
