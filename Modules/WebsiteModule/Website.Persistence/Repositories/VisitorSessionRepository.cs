using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Persistence.Context;
using SharedKernel.Multitenancy;

namespace Website.Persistence.Repositories
{
    public class VisitorSessionRepository : IVisitorSessionRepository
    {
        private readonly WebsiteDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public VisitorSessionRepository(WebsiteDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        public async Task<WebsiteVisitorSession?> GetBySessionIdAsync(string sessionId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<WebsiteVisitorSession>()
           
                .FirstOrDefaultAsync(x => x.SessionId == sessionId, cancellationToken);
        }

        public async Task CreateAsync(WebsiteVisitorSession session, CancellationToken cancellationToken = default)
        {
            await _context.Set<WebsiteVisitorSession>().AddAsync(session, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateLastSeenAsync(WebsiteVisitorSession session, CancellationToken cancellationToken = default)
        {
            session.LastSeenAt = DateTime.UtcNow;
            _context.Set<WebsiteVisitorSession>().Update(session);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> GetActiveSessionsCountAsync(CancellationToken cancellationToken = default)
        {
            var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5);
            return await _context.Set<WebsiteVisitorSession>()
                .AsNoTracking()
                .Where(x => x.LastSeenAt >= fiveMinutesAgo)
                .CountAsync(cancellationToken);
        }
    }
}
