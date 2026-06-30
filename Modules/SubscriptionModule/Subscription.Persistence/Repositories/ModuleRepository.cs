using Microsoft.EntityFrameworkCore;
using Subscription.Application.Contracts.Persistence;
using Subscription.Domain.Entities;
using Subscription.Persistence.Context;

namespace Subscription.Persistence.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly SubscriptionDbContext _context;

        public ModuleRepository(SubscriptionDbContext context)
        {
            _context = context;
        }

        public async Task<Module?> GetByIdAsync(string id)
        {
            return await _context.Modules.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Module?> GetByCodeAsync(string code)
        {
            return await _context.Modules.FirstOrDefaultAsync(m => m.Code == code);
        }

        public async Task<List<Module>> GetAllActiveAsync()
        {
            return await _context.Modules
                .Where(m => m.IsActive)
                .ToListAsync();
        }
    }
}
