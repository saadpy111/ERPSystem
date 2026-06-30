using MediatR;
using SharedKernel.Contracts;
using SharedKernel.Website;
using Website.Application.Contracts.Persistence;
using Website.Domain.Entities;
using Website.Domain.Enums;
using Website.Domain.ValueObjects;
using TenantWebsiteEntity = Website.Domain.Entities.TenantWebsite;

namespace Website.Application.Features.WebsiteInitialization.Commands.InitializeWebsite
{
    public class InitializeWebsiteCommandHandler : IRequestHandler<InitializeWebsiteCommand, InitializeWebsiteResponse>
    {
        private readonly ITenantReadService _tenantReadService;
        private readonly ITenantWebsiteRepository _tenantWebsiteRepository;
        private readonly IThemeRepository _themeRepository;
        private readonly IWebsiteImageService _websiteImageService;
        private readonly IWebsiteUnitOfWork _unitOfWork;

        public InitializeWebsiteCommandHandler(
            ITenantReadService tenantReadService,
            ITenantWebsiteRepository tenantWebsiteRepository,
            IThemeRepository themeRepository,
            IWebsiteImageService websiteImageService,
            IWebsiteUnitOfWork unitOfWork)
        {
            _tenantReadService = tenantReadService;
            _tenantWebsiteRepository = tenantWebsiteRepository;
            _themeRepository = themeRepository;
            _websiteImageService = websiteImageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<InitializeWebsiteResponse> Handle(InitializeWebsiteCommand request, CancellationToken cancellationToken)
        {
            // ===== VERIFY TENANT EXISTS AND IS ACTIVE =====
            var tenantInfo = await _tenantReadService.GetTenantInfoAsync(request.TenantId);
            if (tenantInfo == null)
                return new InitializeWebsiteResponse { Success = false, Error = "Tenant not found." };

            if (!tenantInfo.IsActive)
                return new InitializeWebsiteResponse { Success = false, Error = "Tenant is not active." };

            // ===== VERIFY WEBSITE NOT ALREADY INITIALIZED =====
            if (await _tenantWebsiteRepository.ExistsAsync(request.TenantId))
                return new InitializeWebsiteResponse { Success = false, Error = "Website already initialized for this tenant." };

            // ===== PROCESS IMAGES =====
            string logoUrl = string.Empty;
            string heroBackgroundImageUrl = string.Empty;
            string contactUsImgUrl = string.Empty;
            string clientOImgUrl = string.Empty;

            if (request.Logo != null)
                logoUrl = await _websiteImageService.ProcessWebsiteLogoAsync(request.TenantId, request.Logo);

            if (string.IsNullOrEmpty(request.ThemeCode) && request.HeroBackgroundImage != null)
                heroBackgroundImageUrl = await _websiteImageService.ProcessWebsiteHeroImageAsync(request.TenantId, request.HeroBackgroundImage);

            if (string.IsNullOrEmpty(request.ThemeCode) && request.ContactUsImg != null)
                contactUsImgUrl = await _websiteImageService.ProcessWebsiteContactUsImgAsync(request.TenantId, request.ContactUsImg);

            if (string.IsNullOrEmpty(request.ThemeCode) && request.ClientOImg != null)
                clientOImgUrl = await _websiteImageService.ProcessWebsiteClientOImgAsync(request.TenantId, request.ClientOImg);

            // ===== CREATE TENANT WEBSITE =====
            Theme? theme = null;
            var hasThemeCode = !string.IsNullOrWhiteSpace(request.ThemeCode);

            if (hasThemeCode)
                theme = await _themeRepository.GetByCodeAsync(request.ThemeCode);

            var isValidActiveTheme = hasThemeCode && theme != null && theme.IsActive;

            TenantWebsiteEntity tenantWebsite;

            if (isValidActiveTheme)
            {
                tenantWebsite = new TenantWebsiteEntity
                {
                    Id = Guid.NewGuid(),
                    TenantId = request.TenantId,
                    Mode = WebsiteMode.Theme,
                    ThemeId = theme!.Id,
                    IsPublished = true,
                    Config = new SiteConfig
                    {
                        SiteName = request.SiteName,
                        Domain = request.Domain,
                        BusinessType = request.BusinessType,
                        LogoUrl = logoUrl,
                        about_the_site = request.about_the_site,
                        location = request.location,
                        phone = request.phone,
                        email = request.email,
                        Colors = SnapshotColors(theme.Config?.Colors),
                        Hero = SnapshotHero(theme.Config?.Hero),
                        ContactUsImages = SnapshotContactUsImages(theme.Config?.ContactUsImages),
                        Sections = theme.Config?.Sections?
                            .Select(SnapshotSection)
                            .ToList() ?? new()
                    }
                };
            }
            else
            {
                tenantWebsite = new TenantWebsiteEntity
                {
                    Id = Guid.NewGuid(),
                    TenantId = request.TenantId,
                    Mode = WebsiteMode.Custom,
                    ThemeId = null,
                    IsPublished = true,
                    Config = new SiteConfig
                    {
                        SiteName = request.SiteName,
                        Domain = request.Domain,
                        BusinessType = request.BusinessType,
                        LogoUrl = logoUrl,
                        about_the_site = request.about_the_site,
                        location = request.location,
                        phone = request.phone,
                        email = request.email,
                        Colors = MapColors(request),
                        Hero = MapHero(request, heroBackgroundImageUrl),
                        ContactUsImages = MapContactUsImages(request, contactUsImgUrl, clientOImgUrl),
                        Sections = request.Sections?.Select(MapRequestSection).ToList() ?? new()
                    }
                };
            }

            await _tenantWebsiteRepository.CreateAsync(tenantWebsite);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new InitializeWebsiteResponse
            {
                Success = true,
                WebsiteId = tenantWebsite.Id
            };
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

        private static ThemeColors MapColors(InitializeWebsiteCommand request) => new()
        {
            Primary = request.PrimaryColor,
            Secondary = request.SecondaryColor ?? string.Empty,
            Background = request.BackgroundColor ?? string.Empty,
            Text = request.TextColor ?? string.Empty,
            FontFamily = request.FontFamily ?? "Neo Sans Arabic"
        };

        private static HeroSection MapHero(InitializeWebsiteCommand request, string heroBackgroundImageUrl)
        {
            if (string.IsNullOrEmpty(request.HeroTitle))
                return new HeroSection();

            return new HeroSection
            {
                Title = new TextContent
                {
                    Text = request.HeroTitle,
                    Style = new TextStyle
                    {
                        FontSize = request.HeroTitleFontSize ?? 16,
                        FontWeight = MapFontWeight(request.HeroTitleFontWeight),
                        Color = request.HeroTitleColor ?? "#000000",
                        Alignment = MapTextAlign(request.HeroTitleAlignment),
                        HorizontalSpacing = request.HeroTitleHorizontalSpacing ?? 0,
                        VerticalSpacing = request.HeroTitleVerticalSpacing ?? 0
                    }
                },
                Subtitle = new TextContent
                {
                    Text = request.HeroSubtitle ?? string.Empty,
                    Style = new TextStyle
                    {
                        FontSize = request.HeroSubtitleFontSize ?? 16,
                        FontWeight = MapFontWeight(request.HeroSubtitleFontWeight),
                        Color = request.HeroSubtitleColor ?? "#000000",
                        Alignment = MapTextAlign(request.HeroSubtitleAlignment),
                        HorizontalSpacing = request.HeroSubtitleHorizontalSpacing ?? 0,
                        VerticalSpacing = request.HeroSubtitleVerticalSpacing ?? 0
                    }
                },
                ButtonText = new TextContent
                {
                    Text = request.HeroButtonText ?? string.Empty,
                    Style = new TextStyle
                    {
                        FontSize = request.HeroButtonTextFontSize ?? 16,
                        FontWeight = MapFontWeight(request.HeroButtonTextFontWeight),
                        Color = request.HeroButtonTextColor ?? "#000000",
                        Alignment = MapTextAlign(request.HeroButtonTextAlignment),
                        HorizontalSpacing = request.HeroButtonTextHorizontalSpacing ?? 0,
                        VerticalSpacing = request.HeroButtonTextVerticalSpacing ?? 0
                    }
                },
                BackgroundImage = new ImageContent
                {
                    Url = heroBackgroundImageUrl,
                    Style = new ImageStyle
                    {
                        BorderRadius = request.HeroBackgroundBorderRadius ?? 6,
                        OverlayColor = request.HeroBackgroundOverlayColor ?? "#FFFFFF",
                        OverlayOpacity = request.HeroBackgroundOverlayOpacity ?? 40
                    }
                }
            };
        }

        private static ContactUsImages MapContactUsImages(InitializeWebsiteCommand request, string contactUsImgUrl, string clientOImgUrl)
        {
            if (string.IsNullOrEmpty(request.ThemeCode) && (request.ContactUsImg != null || request.ClientOImg != null))
            {
                return new ContactUsImages
                {
                    ContactUsImg = string.IsNullOrEmpty(contactUsImgUrl) ? null : new ImageContent { Url = contactUsImgUrl },
                    ClientOImg = string.IsNullOrEmpty(clientOImgUrl) ? null : new ImageContent { Url = clientOImgUrl }
                };
            }

            return new ContactUsImages();
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
    }
}
