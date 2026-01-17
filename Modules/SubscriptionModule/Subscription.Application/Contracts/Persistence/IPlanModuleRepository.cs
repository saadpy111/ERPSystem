using Subscription.Domain.Entities;

namespace Subscription.Application.Contracts.Persistence
{
    public interface IPlanModuleRepository
    {
        Task<List<PlanModule>> GetEnabledModulesAsync(string planId);
        Task<bool> IsModuleEnabledInPlanAsync(string planId, string moduleName);
    }
}
