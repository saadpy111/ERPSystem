using MediatR;
using Website.Application.Contracts.Persistence;
using Website.Domain.Enums;
using Website.Domain.ValueObjects;

namespace Website.Application.Features.TenantWebsite.Commands.UpdateConfig
{
    public class UpdateTenantWebsiteConfigCommandHandler : IRequestHandler<UpdateTenantWebsiteConfigCommand, UpdateTenantWebsiteConfigResponse>
    {
        private readonly ITenantWebsiteRepository _tenantWebsiteRepository;
        private readonly IWebsiteUnitOfWork _unitOfWork;

        public UpdateTenantWebsiteConfigCommandHandler(
            ITenantWebsiteRepository tenantWebsiteRepository,
            IWebsiteUnitOfWork unitOfWork)
        {
            _tenantWebsiteRepository = tenantWebsiteRepository;
            _unitOfWork = unitOfWork;
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
            if (request.LogoUrl != null)
                tenantWebsite.Config.LogoUrl = request.LogoUrl;

            // Update presentation data (if provided, switch to Custom mode)
            bool presentationUpdated = false;
            
            if (request.Colors != null)
            {
                tenantWebsite.Config.Colors = request.Colors;
                presentationUpdated = true;
            }
            if (request.Hero != null)
            {
                tenantWebsite.Config.Hero = request.Hero;
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
