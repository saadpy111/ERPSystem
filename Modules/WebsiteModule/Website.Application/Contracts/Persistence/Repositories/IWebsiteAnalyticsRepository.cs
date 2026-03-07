using System;
using System.Threading;
using System.Threading.Tasks;
using Website.Domain.Entities;

namespace Website.Application.Contracts.Persistence.Repositories
{
    public interface IWebsiteAnalyticsRepository
    {
        Task<WebsiteAnalyticsDaily?> GetTodayAsync(CancellationToken cancellationToken = default);
        Task IncrementVisitorsAsync(CancellationToken cancellationToken = default);
        Task IncrementAddToCartAsync(CancellationToken cancellationToken = default);
        Task IncrementCheckoutAsync(CancellationToken cancellationToken = default);
        Task IncrementOrdersAsync(CancellationToken cancellationToken = default);
        Task AddRevenueAsync(decimal amount, CancellationToken cancellationToken = default);
    }
}
