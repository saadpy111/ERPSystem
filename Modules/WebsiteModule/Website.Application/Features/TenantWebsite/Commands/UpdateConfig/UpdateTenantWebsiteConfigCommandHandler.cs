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

            // Update business data (if provided)
            if (request.SiteName != null)
                tenantWebsite.Config.SiteName = request.SiteName;
            if (request.Domain != null)
                tenantWebsite.Config.Domain = request.Domain;
            if (request.BusinessType != null)
                tenantWebsite.Config.BusinessType = request.BusinessType;
            
            // Handle Logo Upload
            if (request.Logo != null)
            {
                tenantWebsite.Config.LogoUrl = await _websiteImageService.ProcessWebsiteLogoAsync(request.TenantId, request.Logo);
            }

            // Update presentation data (if provided, switch to Custom mode)
            bool presentationUpdated = false;
            
            // Handle Hero Image Upload
            if (request.HeroBackgroundImage != null)
            {
                tenantWebsite.Config.Hero.BackgroundImage = await _websiteImageService.ProcessWebsiteHeroImageAsync(request.TenantId, request.HeroBackgroundImage);
                presentationUpdated = true;
            }

            // Map flattened Colors
            if (request.PrimaryColor != null)
            {
                tenantWebsite.Config.Colors.Primary = request.PrimaryColor;
                tenantWebsite.Config.Colors.Secondary = request.SecondaryColor ?? tenantWebsite.Config.Colors.Secondary;
                tenantWebsite.Config.Colors.Background = request.BackgroundColor ?? tenantWebsite.Config.Colors.Background;
                tenantWebsite.Config.Colors.Text = request.TextColor ?? tenantWebsite.Config.Colors.Text;
                presentationUpdated = true;
            }

            // Map flattened Hero Text
            if (request.HeroTitle != null)
            {
                tenantWebsite.Config.Hero.Title = request.HeroTitle;
                tenantWebsite.Config.Hero.Subtitle = request.HeroSubtitle ?? tenantWebsite.Config.Hero.Subtitle;
                tenantWebsite.Config.Hero.ButtonText = request.HeroButtonText ?? tenantWebsite.Config.Hero.ButtonText;
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
    }
}
