using Identity.Application.Contracts.Persistence;
using SharedKernel.Contracts;

namespace Identity.Application.Services
{
    public class TenantReadService : ITenantReadService
    {
        private readonly ITenantRepository _tenantRepository;

        public TenantReadService(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<TenantInfo?> GetTenantInfoAsync(string tenantId)
        {
            var tenant = await _tenantRepository.GetByIdAsync(tenantId);
            if (tenant == null)
                return null;

            return new TenantInfo(Exists: true, IsActive: tenant.IsActive);
        }
    }
}
