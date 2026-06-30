using Microsoft.EntityFrameworkCore;
using SharedKernel.Enums;
using Subscription.Application.Contracts.Persistence;
using Subscription.Domain.Entities;
using Subscription.Persistence.Context;

namespace Subscription.Persistence.Repositories
{
    public class ModulePriceRepository : IModulePriceRepository
    {
        private readonly SubscriptionDbContext _context;

        public ModulePriceRepository(SubscriptionDbContext context)
        {
            _context = context;
        }

        public async Task<List<ModulePrice>> GetByModuleIdAsync(string moduleId)
        {
            return await _context.ModulePrices
                .Where(mp => mp.ModuleId == moduleId)
                .ToListAsync();
        }

        public async Task<ModulePrice?> GetActivePriceAsync(string moduleId, string currencyCode, BillingInterval interval)
        {
            var now = DateTime.UtcNow;
            return await _context.ModulePrices
                .Where(mp => mp.ModuleId == moduleId &&
                             mp.CurrencyCode == currencyCode &&
                             mp.Interval == interval &&
                             mp.IsActive &&
                             (mp.EffectiveFrom == null || mp.EffectiveFrom <= now) &&
                             (mp.EffectiveTo == null || mp.EffectiveTo >= now))
                .FirstOrDefaultAsync();
        }
    }
}
