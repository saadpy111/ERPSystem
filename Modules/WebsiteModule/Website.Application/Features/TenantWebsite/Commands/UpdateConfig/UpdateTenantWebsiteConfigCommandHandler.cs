using MediatR;
using Website.Application.Contracts.Persistence;
using Website.Domain.Enums;
using Website.Domain.ValueObjects;
using SharedKernel.Website;

namespace Website.Application.Features.TenantWebsite.Commands.UpdateConfig
{
    public class UpdateTenantWebsiteConfigCommandHandler : IRequestHandler<UpdateTenantWebsiteConfigCommand, UpdateTenantWebsiteConfigResponse>
    {
        private readonly ITenantWebsiteRepository _tenantWebsiteRepository;
        private readonly IWebsiteUnitOfWork _unitOfWork;
        private readonly IWebsiteImageService _websiteImageService;

        public UpdateTenantWebsiteConfigCommandHandler(
            ITenantWebsiteRepository tenantWebsiteRepository,
            IWebsiteUnitOfWork unitOfWork,
            IWebsiteImageService websiteImageService)
        {
            _tenantWebsiteRepository = tenantWebsiteRepository;
            _unitOfWork = unitOfWork;
            _websiteImageService = websiteImageService;
        }

        public async Task<UpdateTenantWebsiteConfigResponse> Handle(UpdateTenantWebsiteConfigCommand request, CancellationToken cancellationToken)
        {
            var tenantWebsite = await _tenantWebsiteRepository.GetByTenantIdAsync(request.TenantId);
            EnsureConfigStructure(tenantWebsite);
            if (tenantWebsite == null)
            {
                // Create new with Custom mode
                tenantWebsite = new Domain.Entities.TenantWebsite
                {
                    Id = Guid.NewGuid(),
                    TenantId = request.TenantId,
                    Mode = WebsiteMode.Custom,
                    ThemeId = null,
                    Config = new SiteConfig(),
                    IsPublished = false
                };

                await _tenantWebsiteRepository.CreateAsync(tenantWebsite);
            }

            // ── Business data ────────────────────────────────────────────────────
            if (request.SiteName != null)
                tenantWebsite.Config.SiteName = request.SiteName;
            if (request.Domain != null)
                tenantWebsite.Config.Domain = request.Domain;
            if (request.BusinessType != null)
                tenantWebsite.Config.BusinessType = request.BusinessType;
            if (request.about_the_site != null)
                tenantWebsite.Config.about_the_site = request.about_the_site;
            if (request.location != null)
                tenantWebsite.Config.location = request.location;
            if (request.phone != null)
                tenantWebsite.Config.phone = request.phone;
            if (request.email != null)
                tenantWebsite.Config.email = request.email;
            
            // Handle Logo Upload
            if (request.Logo != null)
            {
                tenantWebsite.Config.LogoUrl = await _websiteImageService.ProcessWebsiteLogoAsync(request.TenantId, request.Logo);
            }

            // ── Presentation data ────────────────────────────────────────────────
            bool presentationUpdated = false;

            // Colors
            if (request.PrimaryColor != null)
            {
                tenantWebsite.Config.Colors.Primary = request.PrimaryColor;
                presentationUpdated = true;
            }
            if (request.SecondaryColor != null)
            {
                tenantWebsite.Config.Colors.Secondary = request.SecondaryColor;
                presentationUpdated = true;
            }
            if (request.BackgroundColor != null)
            {
                tenantWebsite.Config.Colors.Background = request.BackgroundColor;
                presentationUpdated = true;
            }
            if (request.TextColor != null)
            {
                tenantWebsite.Config.Colors.Text = request.TextColor;
                presentationUpdated = true;
            }
            if (request.FontFamily != null)
            {
                tenantWebsite.Config.Colors.FontFamily = request.FontFamily;
                presentationUpdated = true;
            }

            // Handle Hero Background Image Upload
            if (request.HeroBackgroundImage != null)
            {
                tenantWebsite.Config.Hero.BackgroundImage.Url = await _websiteImageService.ProcessWebsiteHeroImageAsync(request.TenantId, request.HeroBackgroundImage);
                presentationUpdated = true;
            }

            // Hero Background Image Style
            if (request.HeroBackgroundBorderRadius.HasValue)
            {
                tenantWebsite.Config.Hero.BackgroundImage.Style.BorderRadius = request.HeroBackgroundBorderRadius.Value;
                presentationUpdated = true;
            }
            if (request.HeroBackgroundOverlayColor != null)
            {
                tenantWebsite.Config.Hero.BackgroundImage.Style.OverlayColor = request.HeroBackgroundOverlayColor;
                presentationUpdated = true;
            }
            if (request.HeroBackgroundOverlayOpacity.HasValue)
            {
                tenantWebsite.Config.Hero.BackgroundImage.Style.OverlayOpacity = request.HeroBackgroundOverlayOpacity.Value;
                presentationUpdated = true;
            }

            // Hero Title
            if (request.HeroTitle != null)
            {
                tenantWebsite.Config.Hero.Title.Text = request.HeroTitle;
                presentationUpdated = true;
            }
            if (request.HeroTitleFontSize.HasValue)
            {
                tenantWebsite.Config.Hero.Title.Style.FontSize = request.HeroTitleFontSize.Value;
                presentationUpdated = true;
            }
            if (request.HeroTitleFontWeight.HasValue)
            {
                tenantWebsite.Config.Hero.Title.Style.FontWeight = request.HeroTitleFontWeight.Value;
                presentationUpdated = true;
            }
            if (request.HeroTitleColor != null)
            {
                tenantWebsite.Config.Hero.Title.Style.Color = request.HeroTitleColor;
                presentationUpdated = true;
            }
            if (request.HeroTitleAlignment.HasValue)
            {
                tenantWebsite.Config.Hero.Title.Style.Alignment = request.HeroTitleAlignment.Value;
                presentationUpdated = true;
            }
            if (request.HeroTitleHorizontalSpacing.HasValue)
            {
                tenantWebsite.Config.Hero.Title.Style.HorizontalSpacing = request.HeroTitleHorizontalSpacing.Value;
                presentationUpdated = true;
            }
            if (request.HeroTitleVerticalSpacing.HasValue)
            {
                tenantWebsite.Config.Hero.Title.Style.VerticalSpacing = request.HeroTitleVerticalSpacing.Value;
                presentationUpdated = true;
            }

            // Hero Subtitle
            if (request.HeroSubtitle != null)
            {
                tenantWebsite.Config.Hero.Subtitle.Text = request.HeroSubtitle;
                presentationUpdated = true;
            }
            if (request.HeroSubtitleFontSize.HasValue)
            {
                tenantWebsite.Config.Hero.Subtitle.Style.FontSize = request.HeroSubtitleFontSize.Value;
                presentationUpdated = true;
            }
            if (request.HeroSubtitleFontWeight.HasValue)
            {
                tenantWebsite.Config.Hero.Subtitle.Style.FontWeight = request.HeroSubtitleFontWeight.Value;
                presentationUpdated = true;
            }
            if (request.HeroSubtitleColor != null)
            {
                tenantWebsite.Config.Hero.Subtitle.Style.Color = request.HeroSubtitleColor;
                presentationUpdated = true;
            }
            if (request.HeroSubtitleAlignment.HasValue)
            {
                tenantWebsite.Config.Hero.Subtitle.Style.Alignment = request.HeroSubtitleAlignment.Value;
                presentationUpdated = true;
            }
            if (request.HeroSubtitleHorizontalSpacing.HasValue)
            {
                tenantWebsite.Config.Hero.Subtitle.Style.HorizontalSpacing = request.HeroSubtitleHorizontalSpacing.Value;
                presentationUpdated = true;
            }
            if (request.HeroSubtitleVerticalSpacing.HasValue)
            {
                tenantWebsite.Config.Hero.Subtitle.Style.VerticalSpacing = request.HeroSubtitleVerticalSpacing.Value;
                presentationUpdated = true;
            }

            // Hero Button Text
            if (request.HeroButtonText != null)
            {
                tenantWebsite.Config.Hero.ButtonText.Text = request.HeroButtonText;
                presentationUpdated = true;
            }
            if (request.HeroButtonTextFontSize.HasValue)
            {
                tenantWebsite.Config.Hero.ButtonText.Style.FontSize = request.HeroButtonTextFontSize.Value;
                presentationUpdated = true;
            }
            if (request.HeroButtonTextFontWeight.HasValue)
            {
                tenantWebsite.Config.Hero.ButtonText.Style.FontWeight = request.HeroButtonTextFontWeight.Value;
                presentationUpdated = true;
            }
            if (request.HeroButtonTextColor != null)
            {
                tenantWebsite.Config.Hero.ButtonText.Style.Color = request.HeroButtonTextColor;
                presentationUpdated = true;
            }
            if (request.HeroButtonTextAlignment.HasValue)
            {
                tenantWebsite.Config.Hero.ButtonText.Style.Alignment = request.HeroButtonTextAlignment.Value;
                presentationUpdated = true;
            }
            if (request.HeroButtonTextHorizontalSpacing.HasValue)
            {
                tenantWebsite.Config.Hero.ButtonText.Style.HorizontalSpacing = request.HeroButtonTextHorizontalSpacing.Value;
                presentationUpdated = true;
            }
            if (request.HeroButtonTextVerticalSpacing.HasValue)
            {
                tenantWebsite.Config.Hero.ButtonText.Style.VerticalSpacing = request.HeroButtonTextVerticalSpacing.Value;
                presentationUpdated = true;
            }

            if (request.Sections != null)
            {
                tenantWebsite.Config.Sections = request.Sections;
                presentationUpdated = true;
            }

            // If presentation data was updated, switch to Custom mode
            if (presentationUpdated)
            {
                tenantWebsite.Mode = WebsiteMode.Custom;
                tenantWebsite.ThemeId = null;
            }

            if (request.IsPublished.HasValue)
                tenantWebsite.IsPublished = request.IsPublished.Value;

            await _tenantWebsiteRepository.UpdateAsync(tenantWebsite);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateTenantWebsiteConfigResponse { Success = true };
        }
        private static void EnsureConfigStructure(Domain.Entities.TenantWebsite site)
        {
            site.Config ??= new SiteConfig();

            site.Config.Colors ??= new ThemeColors();

            site.Config.Hero ??= new HeroSection();

            site.Config.Hero.Title ??= new TextContent();
            site.Config.Hero.Subtitle ??= new TextContent();
            site.Config.Hero.ButtonText ??= new TextContent();

            site.Config.Hero.Title.Style ??= new TextStyle();
            site.Config.Hero.Subtitle.Style ??= new TextStyle();
            site.Config.Hero.ButtonText.Style ??= new TextStyle();

            site.Config.Hero.BackgroundImage ??= new ImageContent();
            site.Config.Hero.BackgroundImage.Style ??= new ImageStyle();

            site.Config.Sections ??= new List<SectionItem>();
        }
    }
}
