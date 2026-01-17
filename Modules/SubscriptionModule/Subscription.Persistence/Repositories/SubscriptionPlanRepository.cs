using Microsoft.EntityFrameworkCore;
using Subscription.Application.Contracts.Persistence;
using Subscription.Domain.Entities;
using Subscription.Persistence.Context;

namespace Subscription.Persistence.Repositories
{
    public class SubscriptionPlanRepository : ISubscriptionPlanRepository
    {
        private readonly SubscriptionDbContext _context;

        public SubscriptionPlanRepository(SubscriptionDbContext context)
        {
            _context = context;
        }

        public async Task<SubscriptionPlan?> GetByIdAsync(string id)
        {
            return await _context.SubscriptionPlans
                .Include(p => p.PlanModules)
                .Include(p => p.Prices)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<SubscriptionPlan?> GetByCodeAsync(string code)
        {
            return await _context.SubscriptionPlans
                .Include(p => p.PlanModules)
                .Include(p => p.Prices)
                .FirstOrDefaultAsync(p => p.Code.ToUpper() == code.ToUpper());
        }

        public async Task<SubscriptionPlan?> GetByNameAsync(string name)
        {
            return await _context.SubscriptionPlans
                .Include(p => p.PlanModules)
                .Include(p => p.Prices)
                .FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<SubscriptionPlan?> GetDefaultTrialPlanAsync()
        {
            return await _context.SubscriptionPlans
                .Include(p => p.PlanModules)
                .Include(p => p.Prices)
                .FirstOrDefaultAsync(p => p.IsTrial && p.IsActive);
        }

        public async Task<List<SubscriptionPlan>> GetAllActivePlansAsync()
        {
            return await _context.SubscriptionPlans
                .Include(p => p.PlanModules)
                .Include(p => p.Prices)
                .Where(p => p.IsActive && p.IsVisible)
                .OrderBy(p => p.SortOrder)
                .ToListAsync();
        }

        public async Task<SubscriptionPlan> CreateAsync(SubscriptionPlan plan)
        {
            await _context.SubscriptionPlans.AddAsync(plan);
            return plan;
        }

        public Task UpdateAsync(SubscriptionPlan plan)
        {
            _context.SubscriptionPlans.Update(plan);
            return Task.CompletedTask;
        }
    }
}
