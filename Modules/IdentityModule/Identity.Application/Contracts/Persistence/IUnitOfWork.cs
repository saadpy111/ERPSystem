namespace Identity.Application.Contracts.Persistence
{
    /// <summary>
    /// Unit of Work pattern - manages transaction boundaries
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Commits all changes to the database as a single transaction
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
