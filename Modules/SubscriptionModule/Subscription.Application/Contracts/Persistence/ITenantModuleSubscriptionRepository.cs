using Subscription.Domain.Entities;
using Subscription.Domain.Enums;

namespace Subscription.Application.Contracts.Persistence
{
    public interface ITenantModuleSubscriptionRepository
    {
        Task<List<TenantModuleSubscription>> GetByTenantIdAsync(string tenantId);
        Task<List<TenantModuleSubscription>> GetActiveByTenantIdAsync(string tenantId);
        Task<TenantModuleSubscription?> FindActiveAsync(string tenantId, string moduleCode);
        Task<TenantModuleSubscription> CreateAsync(TenantModuleSubscription subscription);
        Task UpdateAsync(TenantModuleSubscription subscription);
    }
}
