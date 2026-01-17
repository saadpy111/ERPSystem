namespace Website.Application.Contracts.Persistence.Repositories
{
    /// <summary>
    /// Unit of Work interface for transaction management and repository access.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Get a generic repository for any entity type.
        /// </summary>
        IGenericRepository<T> Repository<T>() where T : class;

        /// <summary>
        /// Save all changes to the database.
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Begin a database transaction.
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commit the current transaction.
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rollback the current transaction.
        /// </summary>
        Task RollbackTransactionAsync();
    }
}
