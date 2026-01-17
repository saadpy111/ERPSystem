using MediatR;

namespace Website.Application.Features.TenantWebsite.Commands.ApplyTheme
{
    /// <summary>
    /// Apply a theme to a tenant's website.
    /// This COPIES the theme config into the tenant's SiteConfig (snapshot).
    /// Business data is preserved.
    /// </summary>
    public class ApplyThemeCommand : IRequest<ApplyThemeResponse>
    {
        public string TenantId { get; set; } = string.Empty;
        public Guid ThemeId { get; set; }
    }

    public class ApplyThemeResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
