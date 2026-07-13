using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Website.Application.Contracts.Persistence;

namespace Subscription.Application.Features.Subscriptions.Queries.GetTenantWebsiteDetails
{
    public class GetTenantWebsiteDetailsQueryHandler : IRequestHandler<GetTenantWebsiteDetailsQuery, GetTenantWebsiteDetailsResponse>
    {
        private readonly ITenantWebsiteRepository _tenantWebsiteRepository;

        public GetTenantWebsiteDetailsQueryHandler(ITenantWebsiteRepository tenantWebsiteRepository)
        {
            _tenantWebsiteRepository = tenantWebsiteRepository;
        }

        public async Task<GetTenantWebsiteDetailsResponse> Handle(GetTenantWebsiteDetailsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.TenantId))
            {
                return new GetTenantWebsiteDetailsResponse
                {
                    Success = false,
                    Error = "Tenant ID is invalid."
                };
            }

            var tenantWebsite = await _tenantWebsiteRepository.GetByTenantIdAsync(request.TenantId);

            if (tenantWebsite == null)
            {
                return new GetTenantWebsiteDetailsResponse
                {
                    Success = true,
                    Data = new TenantWebsiteDetailsDto
                    {
                        HasWebsite = false
                    }
                };
            }

            return new GetTenantWebsiteDetailsResponse
            {
                Success = true,
                Data = new TenantWebsiteDetailsDto
                {
                    HasWebsite = true,
                    Domain = tenantWebsite.Config.Domain,
                    Name = tenantWebsite.Config.SiteName,
                    IsPublished = tenantWebsite.IsPublished
                }
            };
        }
    }
}
