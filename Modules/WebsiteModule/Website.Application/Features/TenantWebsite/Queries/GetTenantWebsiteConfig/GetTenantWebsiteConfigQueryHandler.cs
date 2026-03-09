using MediatR;
using SharedKernel.Core.Files;
using Website.Application.Contracts.Persistence;
using Website.Application.DTOs;
using Website.Domain.ValueObjects;

namespace Website.Application.Features.TenantWebsite.Queries.GetTenantWebsiteConfig
{
    public class GetTenantWebsiteConfigQueryHandler : IRequestHandler<GetTenantWebsiteConfigQuery, GetTenantWebsiteConfigResponse>
    {
        private readonly IFileUrlResolver _fileUrlResolver;
        private readonly ITenantWebsiteRepository _tenantWebsiteRepository;

        public GetTenantWebsiteConfigQueryHandler(IFileUrlResolver fileUrlResolver, ITenantWebsiteRepository tenantWebsiteRepository)
        {
            _fileUrlResolver = fileUrlResolver;
            _tenantWebsiteRepository = tenantWebsiteRepository;
        }

        public async Task<GetTenantWebsiteConfigResponse> Handle(GetTenantWebsiteConfigQuery request, CancellationToken cancellationToken)
        {
            var tenantWebsite = await _tenantWebsiteRepository.GetByTenantIdAsync(request.TenantId);

            if (tenantWebsite == null)
            {
                return new GetTenantWebsiteConfigResponse
                {
                    Success = false,
                    Error = "Website configuration not found for this tenant"
                };
            }

            return new GetTenantWebsiteConfigResponse
            {
                Success = true,
                Config = new TenantWebsiteDto
                {
                    Id = tenantWebsite.Id,
                    TenantId = tenantWebsite.TenantId,
                    Mode = tenantWebsite.Mode,
                    ThemeId = tenantWebsite.ThemeId,
                    Config = MapConfigWithResolvedUrls(tenantWebsite.Config),
                    IsPublished = tenantWebsite.IsPublished,
                    CreatedAt = tenantWebsite.CreatedAt,
                    UpdatedAt = tenantWebsite.UpdatedAt
                }
            };
        }

        /// <summary>
        /// Deep-copies the config while resolving all stored relative paths
        /// to absolute URLs. All new nested fields are preserved faithfully.
        /// </summary>
        private SiteConfig MapConfigWithResolvedUrls(SiteConfig config)
        {
            // Defensive null-handling: ensure nested objects exist even for old JSON
            var hero = config.Hero ?? new HeroSection();
            var title = hero.Title ?? new TextContent();
            var subtitle = hero.Subtitle ?? new TextContent();
            var buttonText = hero.ButtonText ?? new TextContent();
            var bgImage = hero.BackgroundImage ?? new ImageContent();
            var colors = config.Colors ?? new ThemeColors();

            return new SiteConfig
            {
                // ── Business data ────────────────────────────────────────────────
                SiteName = config.SiteName,
                Domain = config.Domain,
                BusinessType = config.BusinessType,
                LogoUrl = _fileUrlResolver.Resolve(config.LogoUrl) ?? string.Empty,
                about_the_site = config.about_the_site,
                location = config.location,
                phone = config.phone,
                email = config.email,

                // ── Colors (including FontFamily) ────────────────────────────────
                Colors = new ThemeColors
                {
                    Primary = colors.Primary,
                    Secondary = colors.Secondary,
                    Background = colors.Background,
                    Text = colors.Text,
                    FontFamily = colors.FontFamily
                },

                // ── Hero (full rich structure) ───────────────────────────────────
                Hero = new HeroSection
                {
                    Title = new TextContent
                    {
                        Text = title.Text,
                        Style = new TextStyle
                        {
                            FontSize = title.Style?.FontSize ?? 16,
                            FontWeight = title.Style?.FontWeight ?? Domain.Enums.FontWeight.Normal,
                            Color = title.Style?.Color ?? "#000000",
                            Alignment = title.Style?.Alignment ?? Domain.Enums.TextAlign.Left,
                            HorizontalSpacing = title.Style?.HorizontalSpacing ?? 0,
                            VerticalSpacing = title.Style?.VerticalSpacing ?? 0
                        }
                    },
                    Subtitle = new TextContent
                    {
                        Text = subtitle.Text,
                        Style = new TextStyle
                        {
                            FontSize = subtitle.Style?.FontSize ?? 16,
                            FontWeight = subtitle.Style?.FontWeight ?? Domain.Enums.FontWeight.Normal,
                            Color = subtitle.Style?.Color ?? "#000000",
                            Alignment = subtitle.Style?.Alignment ?? Domain.Enums.TextAlign.Left,
                            HorizontalSpacing = subtitle.Style?.HorizontalSpacing ?? 0,
                            VerticalSpacing = subtitle.Style?.VerticalSpacing ?? 0
                        }
                    },
                    ButtonText = new TextContent
                    {
                        Text = buttonText.Text,
                        Style = new TextStyle
                        {
                            FontSize = buttonText.Style?.FontSize ?? 16,
                            FontWeight = buttonText.Style?.FontWeight ?? Domain.Enums.FontWeight.Normal,
                            Color = buttonText.Style?.Color ?? "#000000",
                            Alignment = buttonText.Style?.Alignment ?? Domain.Enums.TextAlign.Left,
                            HorizontalSpacing = buttonText.Style?.HorizontalSpacing ?? 0,
                            VerticalSpacing = buttonText.Style?.VerticalSpacing ?? 0
                        }
                    },
                    BackgroundImage = new ImageContent
                    {
                        // Resolve the stored relative path to a full URL
                        Url = _fileUrlResolver.Resolve(bgImage.Url) ?? string.Empty,
                        Style = new ImageStyle
                        {
                            BorderRadius = bgImage.Style?.BorderRadius ?? 6,
                            OverlayColor = bgImage.Style?.OverlayColor ?? "#FFFFFF",
                            OverlayOpacity = bgImage.Style?.OverlayOpacity ?? 40
                        }
                    }
                },

                Sections = config.Sections ?? new()
            };
        }
    }
}
