using Subscription.Application.Contracts.Persistence;
using Subscription.Persistence.Context;

namespace Subscription.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SubscriptionDbContext _context;

        public UnitOfWork(SubscriptionDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
