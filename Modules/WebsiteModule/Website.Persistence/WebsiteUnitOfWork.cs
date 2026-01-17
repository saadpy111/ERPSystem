using Website.Application.Contracts.Persistence;
using Website.Persistence.Context;

namespace Website.Persistence
{
    /// <summary>
    /// Unit of Work implementation for Website module.
    /// </summary>
    public class WebsiteUnitOfWork : IWebsiteUnitOfWork
    {
        private readonly WebsiteDbContext _context;

        public WebsiteUnitOfWork(WebsiteDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
