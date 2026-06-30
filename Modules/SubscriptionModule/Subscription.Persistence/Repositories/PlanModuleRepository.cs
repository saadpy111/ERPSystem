using Microsoft.EntityFrameworkCore;
using Subscription.Application.Contracts.Persistence;
using Subscription.Domain.Entities;
using Subscription.Persistence.Context;

namespace Subscription.Persistence.Repositories
{
    public class PlanModuleRepository : IPlanModuleRepository
    {
        private readonly SubscriptionDbContext _context;

        public PlanModuleRepository(SubscriptionDbContext context)
        {
            _context = context;
        }

        public async Task<List<PlanModule>> GetEnabledModulesAsync(string planId)
        {
            return await _context.PlanModules
                .Include(pm => pm.Module)
                .Where(pm => pm.PlanId == planId && pm.IsEnabled)
                .ToListAsync();
        }

        public async Task<bool> IsModuleEnabledInPlanAsync(string planId, string moduleCode)
        {
            return await _context.PlanModules
                .Include(pm => pm.Module)
                .AnyAsync(pm => pm.PlanId == planId &&
                               pm.Module.Code == moduleCode &&
                               pm.IsEnabled);
        }
    }
}
