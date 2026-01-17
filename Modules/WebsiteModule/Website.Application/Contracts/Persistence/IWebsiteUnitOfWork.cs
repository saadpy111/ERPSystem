using System.Threading.Tasks;

namespace Website.Application.Contracts.Persistence
{
    /// <summary>
    /// Unit of Work interface for Website module.
    /// </summary>
    public interface IWebsiteUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
