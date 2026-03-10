using MediatR;
using Website.Application.Contracts.Persistence;

namespace Website.Application.Features.TenantWebsite.Queries.GetPublishStatus
{
    public class GetWebsitePublishStatusQueryHandler : IRequestHandler<GetWebsitePublishStatusQuery, GetWebsitePublishStatusResponse>
    {
        private readonly ITenantWebsiteRepository _tenantWebsiteRepository;

        public GetWebsitePublishStatusQueryHandler(ITenantWebsiteRepository tenantWebsiteRepository)
        {
            _tenantWebsiteRepository = tenantWebsiteRepository;
        }

        public async Task<GetWebsitePublishStatusResponse> Handle(GetWebsitePublishStatusQuery request, CancellationToken cancellationToken)
        {
            var tenantWebsite = await _tenantWebsiteRepository.GetByTenantIdAsync(request.TenantId);

            if (tenantWebsite == null)
            {
                return new GetWebsitePublishStatusResponse
                {
                    Success = false,
                    Error = "Tenant website configuration not found"
                };
            }

            return new GetWebsitePublishStatusResponse
            {
                Success = true,
                IsPublished = tenantWebsite.IsPublished
            };
        }
    }
}
