using Microsoft.EntityFrameworkCore;
using Subscription.Application.Contracts.Persistence;
using Subscription.Domain.Entities;
using Subscription.Domain.Enums;
using Subscription.Persistence.Context;

namespace Subscription.Persistence.Repositories
{
    public class TenantSubscriptionRepository : ITenantSubscriptionRepository
    {
        private readonly SubscriptionDbContext _context;

        public TenantSubscriptionRepository(SubscriptionDbContext context)
        {
            _context = context;
        }

        public async Task<TenantSubscription?> GetByTenantIdAsync(string tenantId)
        {
            return await _context.TenantSubscriptions
                .Include(ts => ts.Plan)
                    .ThenInclude(p => p.PlanModules)
                .FirstOrDefaultAsync(ts => ts.TenantId == tenantId);
        }

        public async Task<List<TenantSubscription>> GetSubscriptionsByBillingAnchorDayAsync(int day)
        {
            return await _context.TenantSubscriptions
                .Include(ts => ts.Plan)
                .Where(ts => ts.BillingAnchorDay == day &&
                            (ts.Status == SubscriptionStatus.Active ||
                             ts.Status == SubscriptionStatus.Trial))
                .ToListAsync();
        }

        public async Task<TenantSubscription> CreateAsync(TenantSubscription subscription)
        {
            await _context.TenantSubscriptions.AddAsync(subscription);
            return subscription;
        }

        public Task UpdateAsync(TenantSubscription subscription)
        {
            _context.TenantSubscriptions.Update(subscription);
            return Task.CompletedTask;
        }
    }
}
