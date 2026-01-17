using Subscription.Domain.Entities;

namespace Subscription.Application.Contracts.Persistence
{
    public interface ISubscriptionPlanRepository
    {
        Task<SubscriptionPlan?> GetByIdAsync(string id);
        Task<SubscriptionPlan?> GetByCodeAsync(string code);
        Task<SubscriptionPlan?> GetByNameAsync(string name);
        Task<SubscriptionPlan?> GetDefaultTrialPlanAsync();
        Task<List<SubscriptionPlan>> GetAllActivePlansAsync();
        Task<SubscriptionPlan> CreateAsync(SubscriptionPlan plan);
        Task UpdateAsync(SubscriptionPlan plan);
    }
}
