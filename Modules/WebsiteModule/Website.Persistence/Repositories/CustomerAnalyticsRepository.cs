using Microsoft.EntityFrameworkCore;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Persistence.Context;

namespace Website.Persistence.Repositories
{
    public class CustomerAnalyticsRepository : ICustomerAnalyticsRepository
    {
        private readonly WebsiteDbContext _context;

        public CustomerAnalyticsRepository(WebsiteDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerAnalytics?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _context.CustomerAnalytics
               // .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        }

        public async Task CreateAsync(CustomerAnalytics analytics, CancellationToken cancellationToken = default)
        {
            await _context.CustomerAnalytics.AddAsync(analytics, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(CustomerAnalytics analytics, CancellationToken cancellationToken = default)
        {
            _context.CustomerAnalytics.Update(analytics);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
