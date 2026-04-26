using Website.Application.DTOs;
using Website.Domain.Entities;
using Website.Domain.ValueObjects;

namespace Website.Application.Mappers
{
    /// <summary>
    /// Centralized mapper contract for Website configuration objects.
    /// Eliminates duplicated inline mapping logic across Query handlers.
    /// Registered as a scoped DI service.
    /// </summary>
    public interface IWebsiteConfigMapper
    {
        /// <summary>
        /// Maps a Theme entity to a ThemeDto, resolving all image URLs.
        /// Used by GetThemeByIdQuery and GetAllThemesQuery.
        /// </summary>
        ThemeDto MapThemeDto(Theme theme);

        /// <summary>
        /// Deep-copies a SiteConfig while resolving all stored relative image
        /// paths to absolute URLs. Used by GetTenantWebsiteConfigQuery.
        /// </summary>
        SiteConfig MapSiteConfigWithResolvedUrls(SiteConfig config);

        /// <summary>
        /// Maps a single TextContent, applying defaults and including MarginTop.
        /// </summary>
        TextContent MapTextContent(TextContent? src);

        /// <summary>
        /// Maps a single ImageContent, resolving its URL.
        /// </summary>
        ImageContent MapImageContent(ImageContent? src);

        /// <summary>
        /// Maps a single SectionItem including all rich fields (Title, Subtitle,
        /// ButtonText, BackgroundImage) and resolves the background image URL.
        /// </summary>
        SectionItem MapSectionItem(SectionItem? src);
    }
}
