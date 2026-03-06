using Website.Domain.Entities;

namespace Website.Application.Contracts.Persistence.Repositories
{
    public interface ICustomerAnalyticsRepository
    {
        Task<CustomerAnalytics?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task CreateAsync(CustomerAnalytics analytics, CancellationToken cancellationToken = default);
        Task UpdateAsync(CustomerAnalytics analytics, CancellationToken cancellationToken = default);
    }
}
