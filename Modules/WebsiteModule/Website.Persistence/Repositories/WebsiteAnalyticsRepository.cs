using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Persistence.Context;
using SharedKernel.Multitenancy;

namespace Website.Persistence.Repositories
{
    public class WebsiteAnalyticsRepository : IWebsiteAnalyticsRepository
    {
        private readonly WebsiteDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public WebsiteAnalyticsRepository(WebsiteDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        private async Task<WebsiteAnalyticsDaily> GetOrCreateTodayAsync(CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow.Date;
            var tenantId = _tenantProvider.GetTenantId() ?? throw new InvalidOperationException("TenantId is required.");

            var analytics = await _context.Set<WebsiteAnalyticsDaily>()
                .FirstOrDefaultAsync(x => x.Date == today, cancellationToken);

            if (analytics == null)
            {
                analytics = new WebsiteAnalyticsDaily
                {
                    Date = today,
                    TenantId = tenantId
                };
                await _context.Set<WebsiteAnalyticsDaily>().AddAsync(analytics, cancellationToken);
            }

            return analytics;
        }

        public async Task<WebsiteAnalyticsDaily?> GetTodayAsync(CancellationToken cancellationToken = default)
        {
            var today = DateTime.UtcNow.Date;
            return await _context.Set<WebsiteAnalyticsDaily>()
                .FirstOrDefaultAsync(x => x.Date == today, cancellationToken);
        }

        public async Task IncrementVisitorsAsync(CancellationToken cancellationToken = default)
        {
            var analytics = await GetOrCreateTodayAsync(cancellationToken);
            analytics.Visitors++;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task IncrementAddToCartAsync(CancellationToken cancellationToken = default)
        {
            var analytics = await GetOrCreateTodayAsync(cancellationToken);
            analytics.AddToCart++;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task IncrementCheckoutAsync(CancellationToken cancellationToken = default)
        {
            var analytics = await GetOrCreateTodayAsync(cancellationToken);
            analytics.CheckoutStarted++;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task IncrementOrdersAsync(CancellationToken cancellationToken = default)
        {
            var analytics = await GetOrCreateTodayAsync(cancellationToken);
            analytics.OrdersCompleted++;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddRevenueAsync(decimal amount, CancellationToken cancellationToken = default)
        {
            var analytics = await GetOrCreateTodayAsync(cancellationToken);
            analytics.Revenue += amount;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
