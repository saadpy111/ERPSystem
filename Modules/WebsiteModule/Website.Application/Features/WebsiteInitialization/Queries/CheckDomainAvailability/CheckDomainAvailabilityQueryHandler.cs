using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.Contracts.Persistence;

namespace Website.Application.Features.WebsiteInitialization.Queries.CheckDomainAvailability
{
    public class CheckDomainAvailabilityQueryHandler : IRequestHandler<CheckDomainAvailabilityQuery, CheckDomainAvailabilityResponse>
    {
        private readonly ITenantWebsiteRepository _tenantWebsiteRepository;

        public CheckDomainAvailabilityQueryHandler(ITenantWebsiteRepository tenantWebsiteRepository)
        {
            _tenantWebsiteRepository = tenantWebsiteRepository;
        }

        public async Task<CheckDomainAvailabilityResponse> Handle(CheckDomainAvailabilityQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Domain))
            {
                return new CheckDomainAvailabilityResponse
                {
                    Success = false,
                    Error = "Domain name is required.",
                    IsAvailable = false
                };
            }

            var existingWebsite = await _tenantWebsiteRepository.GetByDomainAsync(request.Domain.Trim());

            return new CheckDomainAvailabilityResponse
            {
                Success = true,
                IsAvailable = existingWebsite == null
            };
        }
    }
}
