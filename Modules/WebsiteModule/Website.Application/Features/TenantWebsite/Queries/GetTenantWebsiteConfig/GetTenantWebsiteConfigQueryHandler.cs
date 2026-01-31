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


        private SiteConfig MapConfigWithResolvedUrls(SiteConfig config)
        {
            return new SiteConfig
            {
                SiteName = config.SiteName,
                Domain = config.Domain,
                BusinessType = config.BusinessType,

                LogoUrl = _fileUrlResolver.Resolve(config.LogoUrl) ?? string.Empty,

                Colors = config.Colors,

                Hero = new HeroSection
                {
                    Title = config.Hero.Title,
                    Subtitle = config.Hero.Subtitle,
                    ButtonText = config.Hero.ButtonText,
                    BackgroundImage = _fileUrlResolver.Resolve(config.Hero.BackgroundImage) ?? string.Empty
                },

                Sections = config.Sections
            };
        }

    } }
