using Subscription.Domain.Entities;

namespace Subscription.Application.Contracts.Persistence
{
    public interface ITenantSubscriptionRepository
    {
        Task<TenantSubscription?> GetByTenantIdAsync(string tenantId);
        Task<List<TenantSubscription>> GetSubscriptionsByBillingAnchorDayAsync(int day);
        Task<TenantSubscription> CreateAsync(TenantSubscription subscription);
        Task UpdateAsync(TenantSubscription subscription);
    }
}
