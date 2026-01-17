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
                .Where(pm => pm.PlanId == planId && pm.IsEnabled)
                .ToListAsync();
        }

        public async Task<bool> IsModuleEnabledInPlanAsync(string planId, string moduleName)
        {
            return await _context.PlanModules
                .AnyAsync(pm => pm.PlanId == planId && 
                               pm.ModuleName == moduleName && 
                               pm.IsEnabled);
        }
    }
}
