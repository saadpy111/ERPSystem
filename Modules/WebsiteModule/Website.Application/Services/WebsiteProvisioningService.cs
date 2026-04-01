using SharedKernel.Website;
using Website.Application.Contracts.Persistence;
using Website.Domain.Entities;
using Website.Domain.Enums;
using Website.Domain.ValueObjects;

namespace Website.Application.Services
{
    /// <summary>
    /// Implementation of IWebsiteProvisioningService.
    ///
    /// STRICT BUSINESS RULES (NON-NEGOTIABLE):
    /// ═══════════════════════════════════════════════════════════════
    /// CASE 1: THEME MODE
    /// - Business data: FROM USER
    /// - Colors & Hero: FROM THEME ONLY (full snapshot)
    /// - Sections:
    ///   - From USER if provided
    ///   - Otherwise fallback to THEME
    /// - NO merging of Colors or Hero
    ///
    /// CASE 2: CUSTOM MODE
    /// - Business data: FROM USER
    /// - Presentation data: FROM USER
    /// - Defaults applied for missing nested style fields
    /// </summary>
    public class WebsiteProvisioningService : IWebsiteProvisioningService
    {
        private readonly IThemeRepository _themeRepository;
        private readonly ITenantWebsiteRepository _tenantWebsiteRepository;
        private readonly IWebsiteUnitOfWork _unitOfWork;

    public WebsiteProvisioningService(
        IThemeRepository themeRepository,
        ITenantWebsiteRepository tenantWebsiteRepository,
        IWebsiteUnitOfWork unitOfWork)
        {
            _themeRepository = themeRepository;
            _tenantWebsiteRepository = tenantWebsiteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WebsiteProvisioningResult> InitializeTenantWebsiteAsync(
            string tenantId,
            WebsiteInitializationRequest request)
        {
            if (await _tenantWebsiteRepository.ExistsAsync(tenantId))
                return Fail("Tenant website already exists");

            TenantWebsite tenantWebsite;

            Theme? theme = null;
            var hasThemeCode = !string.IsNullOrWhiteSpace(request.ThemeCode);

            if (hasThemeCode)
            {
                theme = await _themeRepository.GetByCodeAsync(request.ThemeCode);
            }

            var isValidActiveTheme =
                hasThemeCode &&
                theme != null &&
                theme.IsActive;

            if (isValidActiveTheme)
            {
                var sections = request.Sections != null && request.Sections.Any()
                    ? request.Sections.Select(s => new SectionItem
                    {
                        Id = s.Id,
                        Enabled = s.Enabled,
                        Order = s.Order
                    }).ToList()
                    : theme!.Config?.Sections?.Select(s => new SectionItem
                    {
                        Id = s.Id,
                        Enabled = s.Enabled,
                        Order = s.Order
                    }).ToList() ?? new();

                tenantWebsite = new TenantWebsite
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Mode = WebsiteMode.Theme,
                    ThemeId = theme!.Id,
                    IsPublished = true,
                    Config = new SiteConfig
                    {
                        SiteName = request.SiteName,
                        Domain = request.Domain,
                        BusinessType = request.BusinessType,
                        LogoUrl = request.LogoUrl,
                        about_the_site = request.about_the_site,
                        location = request.location,
                        phone = request.phone,
                        email = request.email,

                        Colors = SnapshotColors(theme.Config?.Colors),
                        Hero = SnapshotHero(theme.Config?.Hero),
                        ContactUsImages = SnapshotContactUsImages(theme.Config?.ContactUsImages),
                        Sections = sections
                    }
                };
            }
            else
            {
                tenantWebsite = new TenantWebsite
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Mode = WebsiteMode.Custom,
                    ThemeId = null,
                    IsPublished = true,
                    Config = new SiteConfig
                    {
                        SiteName = request.SiteName,
                        Domain = request.Domain,
                        BusinessType = request.BusinessType,
                        LogoUrl = request.LogoUrl,
                        about_the_site = request.about_the_site,
                        location = request.location,
                        phone = request.phone,
                        email = request.email,

                        Colors = MapColors(request.Colors),
                        Hero = MapHero(request.Hero),
                        ContactUsImages = MapContactUsImages(request.ContactUsImages),
                        Sections = request.Sections?.Select(s => new SectionItem
                        {
                            Id = s.Id,
                            Enabled = s.Enabled,
                            Order = s.Order
                        }).ToList() ?? new()
                    }
                };
            }

            await _tenantWebsiteRepository.CreateAsync(tenantWebsite);
            await _unitOfWork.SaveChangesAsync();

            return new WebsiteProvisioningResult { Success = true };
        }

        private static ThemeColors SnapshotColors(ThemeColors? src) => new()
        {
            Primary = src?.Primary ?? "#000000",
            Secondary = src?.Secondary ?? "#FFFFFF",
            Background = src?.Background ?? "#FFFFFF",
            Text = src?.Text ?? "#000000",
            FontFamily = src?.FontFamily ?? "Neo Sans Arabic"
        };

        private static HeroSection SnapshotHero(HeroSection? src) => new()
        {
            Title = SnapshotTextContent(src?.Title),
            Subtitle = SnapshotTextContent(src?.Subtitle),
            ButtonText = SnapshotTextContent(src?.ButtonText),
            BackgroundImage = SnapshotImageContent(src?.BackgroundImage)
        };

        private static TextContent SnapshotTextContent(TextContent? src)
        {
            if (src == null) return DefaultTextContent();

            return new TextContent
            {
                Text = src.Text,
                Style = new TextStyle
                {
                    FontSize = src.Style?.FontSize ?? 16,
                    FontWeight = src.Style?.FontWeight ?? FontWeight.Normal,
                    Color = src.Style?.Color ?? "#000000",
                    Alignment = src.Style?.Alignment ?? TextAlign.Left,
                    HorizontalSpacing = src.Style?.HorizontalSpacing ?? 0,
                    VerticalSpacing = src.Style?.VerticalSpacing ?? 0
                }
            };
        }

        private static ImageContent SnapshotImageContent(ImageContent? src)
        {
            if (src == null) return DefaultImageContent();

            return new ImageContent
            {
                Url = src.Url,
                Style = new ImageStyle
                {
                    BorderRadius = src.Style?.BorderRadius ?? 6,
                    OverlayColor = src.Style?.OverlayColor ?? "#FFFFFF",
                    OverlayOpacity = src.Style?.OverlayOpacity ?? 40
                }
            };
        }

        private static ContactUsImages SnapshotContactUsImages(ContactUsImages? src) => new()
        {
            ContactUsImg = SnapshotImageContent(src?.ContactUsImg),
            ClientOImg   = SnapshotImageContent(src?.ClientOImg)
        };

        private static ContactUsImages MapContactUsImages(WebsiteContactUsImages? src) => new()
        {
            ContactUsImg = MapImageContent(src?.ContactUsImg),
            ClientOImg   = MapImageContent(src?.ClientOImg)
        };

        private static ThemeColors MapColors(WebsiteColors? src) => new()
        {
            Primary = src?.Primary ?? string.Empty,
            Secondary = src?.Secondary ?? string.Empty,
            Background = src?.Background ?? string.Empty,
            Text = src?.Text ?? string.Empty,
            FontFamily = src?.FontFamily ?? "Neo Sans Arabic"
        };

        private static HeroSection MapHero(WebsiteHero? src)
        {
            if (src == null) return new HeroSection();

            return new HeroSection
            {
                Title = MapTextContent(src.Title),
                Subtitle = MapTextContent(src.Subtitle),
                ButtonText = MapTextContent(src.ButtonText),
                BackgroundImage = MapImageContent(src.BackgroundImage)
            };
        }

        private static TextContent MapTextContent(WebsiteTextContent? src)
        {
            if (src == null) return DefaultTextContent();

            var style = src.Style;

            return new TextContent
            {
                Text = src.Text,
                Style = new TextStyle
                {
                    FontSize = style?.FontSize ?? 16,
                    FontWeight = MapFontWeight(style?.FontWeight),
                    Color = style?.Color ?? "#000000",
                    Alignment = MapTextAlign(style?.Alignment),
                    HorizontalSpacing = style?.HorizontalSpacing ?? 0,
                    VerticalSpacing = style?.VerticalSpacing ?? 0
                }
            };
        }

        private static ImageContent MapImageContent(WebsiteImageContent? src)
        {
            if (src == null) return DefaultImageContent();

            var style = src.Style;

            return new ImageContent
            {
                Url = src.Url,
                Style = new ImageStyle
                {
                    BorderRadius = style?.BorderRadius ?? 6,
                    OverlayColor = style?.OverlayColor ?? "#FFFFFF",
                    OverlayOpacity = style?.OverlayOpacity ?? 40
                }
            };
        }

        private static FontWeight MapFontWeight(WebsiteFontWeight? w) => w switch
        {
            WebsiteFontWeight.Light => FontWeight.Light,
            WebsiteFontWeight.Bold => FontWeight.Bold,
            _ => FontWeight.Normal
        };

        private static TextAlign MapTextAlign(WebsiteTextAlign? a) => a switch
        {
            WebsiteTextAlign.Center => TextAlign.Center,
            WebsiteTextAlign.Right => TextAlign.Right,
            _ => TextAlign.Left
        };

        private static TextContent DefaultTextContent() => new()
        {
            Text = string.Empty,
            Style = new TextStyle
            {
                FontSize = 16,
                FontWeight = FontWeight.Normal,
                Color = "#000000",
                Alignment = TextAlign.Left,
                HorizontalSpacing = 0,
                VerticalSpacing = 0
            }
        };

        private static ImageContent DefaultImageContent() => new()
        {
            Url = string.Empty,
            Style = new ImageStyle
            {
                BorderRadius = 6,
                OverlayColor = "#FFFFFF",
                OverlayOpacity = 40
            }
        };

        private static WebsiteProvisioningResult Fail(string error)
        {
            return new WebsiteProvisioningResult { Success = false, Error = error };
        }
    }

}
