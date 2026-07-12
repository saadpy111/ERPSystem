using MediatR;
using Subscription.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.Subscriptions.Queries.GetSubscriptionDashboard
{
    public class GetSubscriptionDashboardQueryHandler : IRequestHandler<GetSubscriptionDashboardQuery, GetSubscriptionDashboardResponse>
    {
        private readonly ITenantSubscriptionRepository _tenantSubscriptionRepository;
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
        private readonly IPlanModuleRepository _planModuleRepository;
        private readonly ITenantModuleSubscriptionRepository _tenantModuleSubscriptionRepository;

        public GetSubscriptionDashboardQueryHandler(
            ITenantSubscriptionRepository tenantSubscriptionRepository,
            ISubscriptionPlanRepository subscriptionPlanRepository,
            IPlanModuleRepository planModuleRepository,
            ITenantModuleSubscriptionRepository tenantModuleSubscriptionRepository)
        {
            _tenantSubscriptionRepository = tenantSubscriptionRepository;
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _planModuleRepository = planModuleRepository;
            _tenantModuleSubscriptionRepository = tenantModuleSubscriptionRepository;
        }

        public async Task<GetSubscriptionDashboardResponse> Handle(GetSubscriptionDashboardQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Get the tenant's current subscription
                var subscription = await _tenantSubscriptionRepository.GetByTenantIdAsync(request.TenantId);
                if (subscription == null)
                {
                    return new GetSubscriptionDashboardResponse
                    {
                        Success = false,
                        Error = "No subscription found for this tenant."
                    };
                }

                // 2. Get active plan details
                var plan = await _subscriptionPlanRepository.GetByIdAsync(subscription.PlanId);
                if (plan == null)
                {
                    return new GetSubscriptionDashboardResponse
                    {
                        Success = false,
                        Error = $"Subscription references plan ID '{subscription.PlanId}' which could not be found."
                    };
                }

                // 3. Get currently enabled modules in the plan
                var enabledPlanModules = await _planModuleRepository.GetEnabledModulesAsync(subscription.PlanId);

                // 4. Get active purchased modules for the tenant
                var activePurchasedSubs = await _tenantModuleSubscriptionRepository.GetActiveByTenantIdAsync(request.TenantId);

                // Map plan modules
                var planModules = enabledPlanModules
                    .Where(pm => pm.Module != null)
                    .Select(pm => new DashboardModuleDto
                    {
                        Id = pm.Module.Id,
                        Code = pm.Module.Code,
                        Name = pm.Module.Name,
                        DisplayName = pm.Module.DisplayName,
                        Description = pm.Module.Description
                    })
                    .OrderBy(m => m.DisplayName)
                    .ToList();

                // Map purchased modules
                var purchasedModules = activePurchasedSubs
                    .Where(tms => tms.Module != null)
                    .Select(tms => new DashboardModuleDto
                    {
                        Id = tms.Module.Id,
                        Code = tms.Module.Code,
                        Name = tms.Module.Name,
                        DisplayName = tms.Module.DisplayName,
                        Description = tms.Module.Description
                    })
                    .ToList();

                // Combine them to construct the list of all effective modules (plan modules + purchased modules)
                var planCodes = planModules.Select(m => m.Code).ToHashSet(StringComparer.OrdinalIgnoreCase);
                var effectiveModulesList = new List<DashboardModuleDto>(planModules);

                foreach (var pm in purchasedModules)
                {
                    if (!planCodes.Contains(pm.Code))
                    {
                        effectiveModulesList.Add(pm);
                    }
                }

                effectiveModulesList = effectiveModulesList.OrderBy(m => m.DisplayName).ToList();

                var dashboardDto = new SubscriptionDashboardDto
                {
                    PlanName = plan.DisplayName ?? plan.Name,
                    PlanModules = planModules,
                    EffectiveModules = effectiveModulesList
                };

                return new GetSubscriptionDashboardResponse
                {
                    Success = true,
                    Data = dashboardDto
                };
            }
            catch (Exception ex)
            {
                return new GetSubscriptionDashboardResponse
                {
                    Success = false,
                    Error = $"An error occurred while compiling subscription details: {ex.Message}"
                };
            }
        }
    }
}
