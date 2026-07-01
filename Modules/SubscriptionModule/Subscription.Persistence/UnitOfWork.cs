using Microsoft.EntityFrameworkCore.Storage;
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

        public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            var tx = await _context.Database.BeginTransactionAsync(cancellationToken);
            return new EfTransaction(tx);
        }
    }

    internal class EfTransaction : ITransaction
    {
        private readonly IDbContextTransaction _tx;

        public EfTransaction(IDbContextTransaction tx) => _tx = tx;

        public Task CommitAsync(CancellationToken cancellationToken = default)
            => _tx.CommitAsync(cancellationToken);

        public Task RollbackAsync(CancellationToken cancellationToken = default)
            => _tx.RollbackAsync(cancellationToken);

        public void Dispose() => _tx.Dispose();

        public ValueTask DisposeAsync() => _tx.DisposeAsync();
    }
}
