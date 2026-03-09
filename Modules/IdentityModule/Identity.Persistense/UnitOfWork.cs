using Identity.Application.Contracts.Persistence;
using Identity.Persistense.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Identity.Persistense
{
    /// <summary>
    /// Unit of Work implementation - provides transaction control
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IdentityDbContext _context;

        public UnitOfWork(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            return await _context.Database.BeginTransactionAsync(cancellationToken);
        }
    }
}
