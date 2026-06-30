using Subscription.Application.Contracts.Persistence;

namespace Subscription.Application.Services
{
    public interface IEffectiveModuleService
    {
        Task<List<string>> GetEffectiveModulesAsync(string tenantId);
    }

    public class EffectiveModuleService : IEffectiveModuleService
    {
        private readonly ITenantSubscriptionRepository _subscriptionRepository;
        private readonly IPlanModuleRepository _planModuleRepository;
        private readonly ITenantModuleSubscriptionRepository _tenantModuleSubscriptionRepository;

        public EffectiveModuleService(
            ITenantSubscriptionRepository subscriptionRepository,
            IPlanModuleRepository planModuleRepository,
            ITenantModuleSubscriptionRepository tenantModuleSubscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _planModuleRepository = planModuleRepository;
            _tenantModuleSubscriptionRepository = tenantModuleSubscriptionRepository;
        }

        public async Task<List<string>> GetEffectiveModulesAsync(string tenantId)
        {
            var subscription = await _subscriptionRepository.GetByTenantIdAsync(tenantId);

            var planModules = new List<string>();
            if (subscription != null)
            {
                var enabledModules = await _planModuleRepository.GetEnabledModulesAsync(subscription.PlanId);
                planModules = enabledModules.Select(m => m.Module?.Code ?? m.ModuleName).ToList();
            }

            var purchasedModules = await _tenantModuleSubscriptionRepository.GetActiveByTenantIdAsync(tenantId);
            var purchasedCodes = purchasedModules.Select(m => m.Module.Code).ToList();

            return planModules.Union(purchasedCodes).Distinct().ToList();
        }
    }
}
