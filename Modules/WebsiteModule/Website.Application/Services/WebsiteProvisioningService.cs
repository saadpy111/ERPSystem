using SharedKernel.Website;
using Website.Application.Contracts.Persistence;
using Website.Domain.Entities;
using Website.Domain.Enums;
using Website.Domain.ValueObjects;

namespace Website.Application.Services
{
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

            Theme? theme = null;
            var hasThemeCode = !string.IsNullOrWhiteSpace(request.ThemeCode);

            if (hasThemeCode)
                theme = await _themeRepository.GetByCodeAsync(request.ThemeCode);

            var isValidActiveTheme =
                hasThemeCode &&
                theme != null &&
                theme.IsActive;

            TenantWebsite tenantWebsite;

            if (isValidActiveTheme)
            {
                var sections = theme!.Config?.Sections?
                    .Select(SnapshotSection)
                    .ToList() ?? new();

                tenantWebsite = new TenantWebsite
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Mode = WebsiteMode.Theme,
                    ThemeId = theme.Id,
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

                        // ?? ??? ??? ?????
                        Sections = request.Sections?.Select(MapRequestSection).ToList() ?? new()
                    }
                };
            }

            await _tenantWebsiteRepository.CreateAsync(tenantWebsite);
            await _unitOfWork.SaveChangesAsync();

            return new WebsiteProvisioningResult { Success = true };
        }

        // ================= SECTION =================

        private static SectionItem MapRequestSection(WebsiteSection src)
        {
            return new SectionItem
            {
                Id = src.Id,
                Enabled = src.Enabled ?? true,
                Order = src.Order ?? 0,
                Title = MapTextContent(src.Title),
                Subtitle = MapTextContent(src.Subtitle),
                ButtonText = MapTextContent(src.ButtonText),
                BackgroundImage = MapImageContent(src.BackgroundImage)
            };
        }

        private static SectionItem SnapshotSection(SectionItem? src)
        {
            if (src == null) return new SectionItem();

            return new SectionItem
            {
                Id = src.Id,
                Enabled = src.Enabled,
                Order = src.Order,
                Title = SnapshotTextContent(src.Title),
                Subtitle = SnapshotTextContent(src.Subtitle),
                ButtonText = SnapshotTextContent(src.ButtonText),
                BackgroundImage = SnapshotImageContent(src.BackgroundImage)
            };
        }

        // ================= SNAPSHOT =================

        private static ThemeColors SnapshotColors(ThemeColors? src) => new()
        {
            Primary = src?.Primary,
            Secondary = src?.Secondary,
            Background = src?.Background,
            Text = src?.Text,
            FontFamily = src?.FontFamily
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
            return new TextContent
            {
                Text = src?.Text,
                Style = src?.Style == null ? new TextStyle() : new TextStyle
                {
                    FontSize = src.Style.FontSize,
                    FontWeight = src.Style.FontWeight,
                    Color = src.Style.Color,
                    Alignment = src.Style.Alignment,
                    HorizontalSpacing = src.Style.HorizontalSpacing,
                    VerticalSpacing = src.Style.VerticalSpacing,
                    MarginTop = src.Style.MarginTop,
                    BackgroundColor = src.Style.BackgroundColor
                }
            };
        }

        private static ImageContent SnapshotImageContent(ImageContent? src)
        {
            return new ImageContent
            {
                Url = src?.Url,
                Style = src?.Style == null ? new ImageStyle() : new ImageStyle
                {
                    BorderRadius = src.Style.BorderRadius,
                    OverlayColor = src.Style.OverlayColor,
                    OverlayOpacity = src.Style.OverlayOpacity
                }
            };
        }

        private static ContactUsImages SnapshotContactUsImages(ContactUsImages? src) => new()
        {
            ContactUsImg = SnapshotImageContent(src?.ContactUsImg),
            ClientOImg = SnapshotImageContent(src?.ClientOImg)
        };

        // ================= MAP =================

        private static ContactUsImages MapContactUsImages(WebsiteContactUsImages? src) => new()
        {
            ContactUsImg = MapImageContent(src?.ContactUsImg),
            ClientOImg = MapImageContent(src?.ClientOImg)
        };

        private static ThemeColors MapColors(WebsiteColors? src) => new()
        {
            Primary = src?.Primary,
            Secondary = src?.Secondary,
            Background = src?.Background,
            Text = src?.Text,
            FontFamily = src?.FontFamily
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

        private static TextContent? MapTextContent(WebsiteTextContent? src)
        {
            if (src == null) return null;

            return new TextContent
            {
                Text = src.Text,
                Style = src.Style == null ? new TextStyle() : new TextStyle
                {
                    FontSize = src.Style.FontSize,
                    FontWeight = MapFontWeight(src.Style.FontWeight),
                    Color = src.Style.Color,
                    Alignment = MapTextAlign(src.Style.Alignment),
                    HorizontalSpacing = src.Style.HorizontalSpacing,
                    VerticalSpacing = src.Style.VerticalSpacing,
                    MarginTop = src.Style.MarginTop,
                    BackgroundColor = src.Style.BackgroundColor
                }
            };
        }

        private static ImageContent? MapImageContent(WebsiteImageContent? src)
        {
            if (src == null) return null;

            return new ImageContent
            {
                Url = src.Url,
                Style = src.Style == null ? new ImageStyle() : new ImageStyle
                {
                    BorderRadius = src.Style.BorderRadius,
                    OverlayColor = src.Style.OverlayColor,
                    OverlayOpacity = src.Style.OverlayOpacity
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

        private static WebsiteProvisioningResult Fail(string error)
        {
            return new WebsiteProvisioningResult { Success = false, Error = error };
        }
    }

}
