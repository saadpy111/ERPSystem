namespace Subscription.Application.Contracts.Persistence
{
    public interface ITransaction : IDisposable, IAsyncDisposable
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
