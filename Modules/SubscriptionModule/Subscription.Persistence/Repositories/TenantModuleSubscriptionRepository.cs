using Microsoft.EntityFrameworkCore;
using Subscription.Application.Contracts.Persistence;
using Subscription.Domain.Entities;
using Subscription.Domain.Enums;
using Subscription.Persistence.Context;

namespace Subscription.Persistence.Repositories
{
    public class TenantModuleSubscriptionRepository : ITenantModuleSubscriptionRepository
    {
        private readonly SubscriptionDbContext _context;

        public TenantModuleSubscriptionRepository(SubscriptionDbContext context)
        {
            _context = context;
        }

        public async Task<List<TenantModuleSubscription>> GetByTenantIdAsync(string tenantId)
        {
            return await _context.TenantModuleSubscriptions
                .Include(tms => tms.Module)
                .Where(tms => tms.TenantId == tenantId)
                .ToListAsync();
        }

        public async Task<List<TenantModuleSubscription>> GetActiveByTenantIdAsync(string tenantId)
        {
            return await _context.TenantModuleSubscriptions
                .Include(tms => tms.Module)
                .Where(tms => tms.TenantId == tenantId && tms.Status == ModuleSubscriptionStatus.Active)
                .ToListAsync();
        }

        public async Task<TenantModuleSubscription?> FindActiveAsync(string tenantId, string moduleCode)
        {
            return await _context.TenantModuleSubscriptions
                .Include(tms => tms.Module)
                .Where(tms => tms.TenantId == tenantId &&
                              tms.Module.Code == moduleCode &&
                              tms.Status == ModuleSubscriptionStatus.Active)
                .FirstOrDefaultAsync();
        }

        public async Task<TenantModuleSubscription> CreateAsync(TenantModuleSubscription subscription)
        {
            await _context.TenantModuleSubscriptions.AddAsync(subscription);
            return subscription;
        }

        public Task UpdateAsync(TenantModuleSubscription subscription)
        {
            _context.TenantModuleSubscriptions.Update(subscription);
            return Task.CompletedTask;
        }
    }
}
