using SharedKernel.Website;
using Website.Application.Contracts.Persistence;

namespace Website.Application.Services
{
    /// <summary>
    /// Implementation of ITenantDomainResolver.
    /// Resolves tenant information from domain name.
    /// Used by IdentityModule for client authentication.
    /// </summary>
    public class TenantDomainResolver : ITenantDomainResolver
    {
        private readonly ITenantWebsiteRepository _tenantWebsiteRepository;

        public TenantDomainResolver(ITenantWebsiteRepository tenantWebsiteRepository)
        {
            _tenantWebsiteRepository = tenantWebsiteRepository;
        }

        public async Task<TenantDomainResult?> GetTenantByDomainAsync(string domain)
        {
            if (string.IsNullOrWhiteSpace(domain))
                return null;

            var tenantWebsite = await _tenantWebsiteRepository.GetByDomainAsync(domain);
            
            if (tenantWebsite == null)
                return null;

            return new TenantDomainResult
            {
                TenantId = tenantWebsite.TenantId,
                TenantName = tenantWebsite.Config.SiteName,  // Use site name as tenant name
                StoreName = tenantWebsite.Config.SiteName,
                IsPublished = tenantWebsite.IsPublished
            };
        }
    }
}
