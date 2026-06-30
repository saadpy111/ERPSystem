using SharedKernel.Subscription;
using Subscription.Application.Contracts.Persistence;
using Subscription.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Subscription.Application.Services
{
    public class SubscriptionModuleChecker : ISubscriptionModuleChecker
    {
        private readonly ITenantSubscriptionRepository _subscriptionRepository;
        private readonly IPlanModuleRepository _planModuleRepository;
        private readonly ITenantModuleSubscriptionRepository _tenantModuleSubscriptionRepository;
        private readonly IEffectiveModuleService _effectiveModuleService;

        public SubscriptionModuleChecker(
            ITenantSubscriptionRepository subscriptionRepository,
            IPlanModuleRepository planModuleRepository,
            ITenantModuleSubscriptionRepository tenantModuleSubscriptionRepository,
            IEffectiveModuleService effectiveModuleService)
        {
            _subscriptionRepository = subscriptionRepository;
            _planModuleRepository = planModuleRepository;
            _tenantModuleSubscriptionRepository = tenantModuleSubscriptionRepository;
            _effectiveModuleService = effectiveModuleService;
        }

        public async Task<bool> IsModuleEnabledAsync(string tenantId, string moduleCode)
        {
            var subscription = await _subscriptionRepository.GetByTenantIdAsync(tenantId);

            if (subscription == null ||
                (subscription.Status != SubscriptionStatus.Active &&
                 subscription.Status != SubscriptionStatus.Trial))
            {
                return false;
            }

            var inPlan = await _planModuleRepository.IsModuleEnabledInPlanAsync(subscription.PlanId, moduleCode);
            if (inPlan) return true;

            var purchased = await _tenantModuleSubscriptionRepository.FindActiveAsync(tenantId, moduleCode);
            return purchased != null;
        }

        public async Task<List<string>> GetEnabledModulesAsync(string tenantId)
        {
            return await _effectiveModuleService.GetEffectiveModulesAsync(tenantId);
        }

        public async Task<bool> HasActiveSubscriptionAsync(string tenantId)
        {
            var subscription = await _subscriptionRepository.GetByTenantIdAsync(tenantId);

            return subscription != null &&
                   (subscription.Status == SubscriptionStatus.Active ||
                    subscription.Status == SubscriptionStatus.Trial);
        }

        public async Task<SubscriptionStatusDto> GetSubscriptionStatusAsync(string tenantId)
        {
            var subscription = await _subscriptionRepository.GetByTenantIdAsync(tenantId);

            if (subscription == null)
            {
                return new SubscriptionStatusDto
                {
                    IsActive = false,
                    Status = "None"
                };
            }

            return new SubscriptionStatusDto
            {
                IsActive = subscription.Status == SubscriptionStatus.Active ||
                          subscription.Status == SubscriptionStatus.Trial,
                Status = subscription.Status.ToString(),
                ExpiresAt = subscription.TrialEndsAt ?? subscription.CurrentPeriodEnd,
                PlanName = subscription.Plan.DisplayName
            };
        }
    }
}
