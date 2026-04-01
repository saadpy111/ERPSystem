using MediatR;
using Website.Application.Contracts.Persistence;
using Website.Domain.Enums;
using Website.Domain.ValueObjects;

namespace Website.Application.Features.TenantWebsite.Commands.ApplyTheme
{
    /// <summary>
    /// Handler for applying a theme to a tenant's website.
    /// CRITICAL: Theme is COPIED into SiteConfig (snapshot), not live-linked.
    /// Business data is NEVER overridden.
    /// Supports rich text styling and image styling.
    /// </summary>
    public class ApplyThemeCommandHandler : IRequestHandler<ApplyThemeCommand, ApplyThemeResponse>
    {
        private readonly IThemeRepository _themeRepository;
        private readonly ITenantWebsiteRepository _tenantWebsiteRepository;
        private readonly IWebsiteUnitOfWork _unitOfWork;

        public ApplyThemeCommandHandler(
            IThemeRepository themeRepository,
            ITenantWebsiteRepository tenantWebsiteRepository,
            IWebsiteUnitOfWork unitOfWork)
        {
            _themeRepository = themeRepository;
            _tenantWebsiteRepository = tenantWebsiteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApplyThemeResponse> Handle(ApplyThemeCommand request, CancellationToken cancellationToken)
        {
            // Step 1: Load and validate theme
            var theme = await _themeRepository.GetByIdAsync(request.ThemeId);

            if (theme == null)
            {
                return new ApplyThemeResponse
                {
                    Success = false,
                    Error = "Theme not found"
                };
            }

            if (!theme.IsActive)
            {
                return new ApplyThemeResponse
                {
                    Success = false,
                    Error = "Theme is not active"
                };
            }

            // Step 2: Get or create tenant website
            var tenantWebsite = await _tenantWebsiteRepository.GetByTenantIdAsync(request.TenantId);

            if (tenantWebsite == null)
            {
                tenantWebsite = new Domain.Entities.TenantWebsite
                {
                    Id = Guid.NewGuid(),
                    TenantId = request.TenantId,
                    Mode = WebsiteMode.Theme,
                    ThemeId = theme.Id,
                    Config = new SiteConfig
                    {
                        // Business data starts empty
                        SiteName = string.Empty,
                        Domain = string.Empty,
                        BusinessType = string.Empty,
                        LogoUrl = string.Empty,
                        about_the_site = string.Empty,
                        location = string.Empty,
                        phone = string.Empty,
                        email = string.Empty,

                        // Presentation snapshot
                        Colors = SnapshotColors(theme.Config?.Colors),
                        Hero = SnapshotHero(theme.Config?.Hero),
                        ContactUsImages = SnapshotContactUsImages(theme.Config?.ContactUsImages),
                        Sections = theme.Config?.Sections?.Select(s => new SectionItem
                        {
                            Id = s.Id,
                            Enabled = s.Enabled,
                            Order = s.Order
                        }).ToList() ?? new()
                    },
                    IsPublished = false
                };

                await _tenantWebsiteRepository.CreateAsync(tenantWebsite);
            }
            else
            {
                // Preserve business data
                tenantWebsite.Mode = WebsiteMode.Theme;
                tenantWebsite.ThemeId = theme.Id;

                tenantWebsite.Config.Colors = SnapshotColors(theme.Config?.Colors);
                tenantWebsite.Config.Hero = SnapshotHero(theme.Config?.Hero);
                tenantWebsite.Config.ContactUsImages = SnapshotContactUsImages(theme.Config?.ContactUsImages);

                tenantWebsite.Config.Sections = theme.Config?.Sections?.Select(s => new SectionItem
                {
                    Id = s.Id,
                    Enabled = s.Enabled,
                    Order = s.Order
                }).ToList() ?? new();

                await _tenantWebsiteRepository.UpdateAsync(tenantWebsite);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ApplyThemeResponse { Success = true };
        }

        // Snapshot helpers


        private static ThemeColors SnapshotColors(ThemeColors? src) => new()
        {
            Primary = src?.Primary ?? "#000000",
            Secondary = src?.Secondary ?? "#ffffff",
            Background = src?.Background ?? "#ffffff",
            Text = src?.Text ?? "#000000",
            FontFamily = src?.FontFamily ?? "Default"
        };

        private static HeroSection SnapshotHero(HeroSection? src) => new()
        {
            Title = SnapshotTextContent(src?.Title),
            Subtitle = SnapshotTextContent(src?.Subtitle),
            ButtonText = SnapshotTextContent(src?.ButtonText),
            BackgroundImage = SnapshotImageContent(src?.BackgroundImage)
        };

        private static TextContent SnapshotTextContent(TextContent? src) => new()
        {
            Text = src?.Text ?? string.Empty,
            Style = new TextStyle
            {
                FontSize = src?.Style?.FontSize ?? 16,
                FontWeight = src?.Style?.FontWeight ?? FontWeight.Normal,
                Color = src?.Style?.Color ?? "#000000",
                Alignment = src?.Style?.Alignment ?? TextAlign.Left,
                HorizontalSpacing = src?.Style?.HorizontalSpacing ?? 0,
                VerticalSpacing = src?.Style?.VerticalSpacing ?? 0
            }
        };

        private static ImageContent SnapshotImageContent(ImageContent? src) => new()
        {
            Url = src?.Url ?? string.Empty,
            Style = new ImageStyle
            {
                BorderRadius = src?.Style?.BorderRadius ?? 6,
                OverlayColor = src?.Style?.OverlayColor ?? "#FFFFFF",
                OverlayOpacity = src?.Style?.OverlayOpacity ?? 40
            }
        };

        private static ContactUsImages SnapshotContactUsImages(ContactUsImages? src) => new()
        {
            ContactUsImg = SnapshotImageContent(src?.ContactUsImg),
            ClientOImg   = SnapshotImageContent(src?.ClientOImg)
        };
    }
}