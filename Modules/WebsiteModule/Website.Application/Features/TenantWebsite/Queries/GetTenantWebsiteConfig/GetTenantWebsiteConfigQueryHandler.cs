using MediatR;
using Website.Application.Contracts.Persistence;
using Website.Application.DTOs;
using Website.Application.Mappers;

namespace Website.Application.Features.TenantWebsite.Queries.GetTenantWebsiteConfig
{
    public class GetTenantWebsiteConfigQueryHandler
        : IRequestHandler<GetTenantWebsiteConfigQuery, GetTenantWebsiteConfigResponse>
    {
        private readonly ITenantWebsiteRepository _tenantWebsiteRepository;
        private readonly IWebsiteConfigMapper _mapper;

        public GetTenantWebsiteConfigQueryHandler(
            ITenantWebsiteRepository tenantWebsiteRepository,
            IWebsiteConfigMapper mapper)
        {
            _tenantWebsiteRepository = tenantWebsiteRepository;
            _mapper                  = mapper;
        }

        public async Task<GetTenantWebsiteConfigResponse> Handle(
            GetTenantWebsiteConfigQuery request, CancellationToken cancellationToken)
        {
            var tenantWebsite = await _tenantWebsiteRepository.GetByTenantIdAsync(request.TenantId);

            if (tenantWebsite == null)
            {
                return new GetTenantWebsiteConfigResponse
                {
                    Success = false,
                    Error   = "Website configuration not found for this tenant"
                };
            }

            return new GetTenantWebsiteConfigResponse
            {
                Success = true,
                Config  = new TenantWebsiteDto
                {
                    Id          = tenantWebsite.Id,
                    TenantId    = tenantWebsite.TenantId,
                    Mode        = tenantWebsite.Mode,
                    ThemeId     = tenantWebsite.ThemeId,
                    Config      = _mapper.MapSiteConfigWithResolvedUrls(tenantWebsite.Config),
                    IsPublished = tenantWebsite.IsPublished,
                    CreatedAt   = tenantWebsite.CreatedAt,
                    UpdatedAt   = tenantWebsite.UpdatedAt
                }
            };
        }
    }
}
