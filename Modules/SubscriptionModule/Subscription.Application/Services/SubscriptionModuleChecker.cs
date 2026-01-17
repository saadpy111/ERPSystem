using SharedKernel.Subscription;
using Subscription.Application.Contracts.Persistence;
using Subscription.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Subscription.Application.Services
{
    /// <summary>
    /// Implements ISubscriptionModuleChecker for cross-module consumption by Identity module.
    /// Checks if modules are enabled in tenant subscriptions.
    /// </summary>
    public class SubscriptionModuleChecker : ISubscriptionModuleChecker
    {
        private readonly ITenantSubscriptionRepository _subscriptionRepository;
        private readonly IPlanModuleRepository _planModuleRepository;

        public SubscriptionModuleChecker(
            ITenantSubscriptionRepository subscriptionRepository,
            IPlanModuleRepository planModuleRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _planModuleRepository = planModuleRepository;
        }

        public async Task<bool> IsModuleEnabledAsync(string tenantId, string moduleName)
        {
            var subscription = await _subscriptionRepository.GetByTenantIdAsync(tenantId);
            
            if (subscription == null ||
                (subscription.Status != SubscriptionStatus.Active &&
                 subscription.Status != SubscriptionStatus.Trial))
            {
                return false;
            }

            return await _planModuleRepository.IsModuleEnabledInPlanAsync(
                subscription.PlanId,
                moduleName);
        }

        public async Task<List<string>> GetEnabledModulesAsync(string tenantId)
        {
            var subscription = await _subscriptionRepository.GetByTenantIdAsync(tenantId);
            
            if (subscription == null)
                return new List<string>();

            var modules = await _planModuleRepository.GetEnabledModulesAsync(subscription.PlanId);
            return modules.Select(m => m.ModuleName).ToList();
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
