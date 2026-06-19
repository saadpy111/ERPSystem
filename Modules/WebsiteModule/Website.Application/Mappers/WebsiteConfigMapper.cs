using SharedKernel.Core.Files;
using Website.Application.DTOs;
using Website.Domain.Entities;
using Website.Domain.Enums;
using Website.Domain.ValueObjects;

namespace Website.Application.Mappers
{
    /// <summary>
    /// Centralized implementation of all Website configuration mapping logic.
    /// Injected via DI (scoped) into Query handlers.
    /// Single source of truth for: TextContent, ImageContent, HeroSection,
    /// SectionItem (rich), ContactUsImages, ThemeColors, and SiteConfig mapping.
    /// </summary>
    public sealed class WebsiteConfigMapper : IWebsiteConfigMapper
    {
        private readonly IFileUrlResolver _fileUrlResolver;

        public WebsiteConfigMapper(IFileUrlResolver fileUrlResolver)
        {
            _fileUrlResolver = fileUrlResolver;
        }

        // ─────────────────────────────────────────────────────────────────────────
        //  Theme
        // ─────────────────────────────────────────────────────────────────────────

        /// <inheritdoc/>
        public ThemeDto MapThemeDto(Theme theme)
        {
            var config = theme.Config ?? new ThemeConfig();

            return new ThemeDto
            {
                Id           = theme.Id,
                Code         = theme.Code,
                Name         = theme.Name,
                PreviewImage = _fileUrlResolver.Resolve(theme.PreviewImage) ?? theme.PreviewImage,
                IsActive     = theme.IsActive,
                Config       = MapThemeConfig(config),
                CreatedAt    = theme.CreatedAt,
                UpdatedAt    = theme.UpdatedAt
            };
        }

        // ─────────────────────────────────────────────────────────────────────────
        //  SiteConfig (TenantWebsite)
        // ─────────────────────────────────────────────────────────────────────────

        /// <inheritdoc/>
        public SiteConfig MapSiteConfigWithResolvedUrls(SiteConfig config)
        {
            var colors = config.Colors ?? new ThemeColors();

            return new SiteConfig
            {
                // ── Business data (pass-through, only LogoUrl needs resolving) ──
                SiteName       = config.SiteName,
                Domain         = config.Domain,
                BusinessType   = config.BusinessType,
                LogoUrl        = _fileUrlResolver.Resolve(config.LogoUrl) ?? string.Empty,
                about_the_site = config.about_the_site,
                location       = config.location,
                phone          = config.phone,
                email          = config.email,

                // ── Presentation data ──────────────────────────────────────────
                Colors         = MapThemeColors(colors),
                Hero           = MapHeroSection(config.Hero),
                Sections       = (config.Sections ?? new())
                                    .Select(MapSectionItem)
                                    .ToList(),
                ContactUsImages = MapContactUsImages(config.ContactUsImages)
            };
        }

        // ─────────────────────────────────────────────────────────────────────────
        //  Primitive mappers (public – satisfy interface; usable independently)
        // ─────────────────────────────────────────────────────────────────────────

        /// <inheritdoc/>
        public TextContent MapTextContent(TextContent? src)
        {
            var style = src?.Style ?? new TextStyle();

            return new TextContent
            {
                Text  = src?.Text ?? string.Empty,
                Style = new TextStyle
                {
                    FontSize          = style.FontSize == 0 ? 16 : style.FontSize,
                    FontWeight        = style.FontWeight,
                    Color             = style.Color ?? "#000000",
                    Alignment         = style.Alignment,
                    HorizontalSpacing = style.HorizontalSpacing,
                    VerticalSpacing   = style.VerticalSpacing,
                    MarginTop         = style.MarginTop,   // defaults to 0 for old data
                    BackgroundColor   = style.BackgroundColor
                }
            };
        }

        /// <inheritdoc/>
        public ImageContent MapImageContent(ImageContent? src)
        {
            var style = src?.Style ?? new ImageStyle();

            return new ImageContent
            {
                Url   = _fileUrlResolver.Resolve(src?.Url) ?? src?.Url ?? string.Empty,
                Style = new ImageStyle
                {
                    BorderRadius   = style.BorderRadius,
                    OverlayColor   = style.OverlayColor ?? "#FFFFFF",
                    OverlayOpacity = style.OverlayOpacity
                }
            };
        }

        /// <inheritdoc/>
        public SectionItem MapSectionItem(SectionItem? src)
        {
            if (src is null) return DefaultSectionItem();

            return new SectionItem
            {
                Id              = src.Id,
                Enabled         = src.Enabled,
                Order           = src.Order,
                Title           = MapTextContent(src.Title),
                Subtitle        = MapTextContent(src.Subtitle),
                ButtonText      = MapTextContent(src.ButtonText),
                BackgroundImage = MapImageContent(src.BackgroundImage)
            };
        }

        // ─────────────────────────────────────────────────────────────────────────
        //  Private helpers
        // ─────────────────────────────────────────────────────────────────────────

        private ThemeConfig MapThemeConfig(ThemeConfig config) => new()
        {
            Colors          = MapThemeColors(config.Colors),
            Hero            = MapHeroSection(config.Hero),
            Sections        = (config.Sections ?? new())
                                  .Select(MapSectionItem)
                                  .ToList(),
            ContactUsImages = MapContactUsImages(config.ContactUsImages)
        };

        private HeroSection MapHeroSection(HeroSection? src) => new()
        {
            Title           = MapTextContent(src?.Title),
            Subtitle        = MapTextContent(src?.Subtitle),
            ButtonText      = MapTextContent(src?.ButtonText),
            BackgroundImage = MapImageContent(src?.BackgroundImage)
        };

        private ContactUsImages MapContactUsImages(ContactUsImages? src) => new()
        {
            ContactUsImg = MapImageContent(src?.ContactUsImg),
            ClientOImg   = MapImageContent(src?.ClientOImg)
        };

        private static ThemeColors MapThemeColors(ThemeColors? src) => new()
        {
            Primary    = src?.Primary    ?? string.Empty,
            Secondary  = src?.Secondary  ?? string.Empty,
            Background = src?.Background ?? string.Empty,
            Text       = src?.Text       ?? string.Empty,
            FontFamily = src?.FontFamily ?? "Neo Sans Arabic"
        };

        private static SectionItem DefaultSectionItem() => new()
        {
            Id              = string.Empty,
            Enabled         = true,
            Order           = 0,
            Title           = DefaultTextContent(),
            Subtitle        = DefaultTextContent(),
            ButtonText      = DefaultTextContent(),
            BackgroundImage = DefaultImageContent()
        };

        private static TextContent DefaultTextContent() => new()
        {
            Text  = string.Empty,
            Style = new TextStyle
            {
                FontSize          = 16,
                FontWeight        = FontWeight.Normal,
                Color             = "#000000",
                Alignment         = TextAlign.Left,
                HorizontalSpacing = 0,
                VerticalSpacing   = 0,
                MarginTop         = 0,
                BackgroundColor   = null
            }
        };

        private static ImageContent DefaultImageContent() => new()
        {
            Url   = string.Empty,
            Style = new ImageStyle
            {
                BorderRadius   = 6,
                OverlayColor   = "#FFFFFF",
                OverlayOpacity = 40
            }
        };
    }
}
