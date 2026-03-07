using System;
using System.Threading;
using System.Threading.Tasks;
using Website.Domain.Entities;

namespace Website.Application.Contracts.Persistence.Repositories
{
    public interface IVisitorSessionRepository
    {
        Task<WebsiteVisitorSession?> GetBySessionIdAsync(string sessionId, CancellationToken cancellationToken = default);
        Task CreateAsync(WebsiteVisitorSession session, CancellationToken cancellationToken = default);
        Task UpdateLastSeenAsync(WebsiteVisitorSession session, CancellationToken cancellationToken = default);
        Task<int> GetActiveSessionsCountAsync(CancellationToken cancellationToken = default);
    }
}
