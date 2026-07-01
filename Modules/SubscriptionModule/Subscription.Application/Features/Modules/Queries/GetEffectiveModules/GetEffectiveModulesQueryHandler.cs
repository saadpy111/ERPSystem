using MediatR;
using Subscription.Application.Contracts.Persistence;

namespace Subscription.Application.Features.Modules.Queries.GetEffectiveModules
{
    public class GetEffectiveModulesQueryHandler : IRequestHandler<GetEffectiveModulesQuery, GetEffectiveModulesResponse>
    {
        private readonly ITenantSubscriptionRepository _tenantSubscriptionRepository;
        private readonly IPlanModuleRepository _planModuleRepository;
        private readonly ITenantModuleSubscriptionRepository _tenantModuleSubscriptionRepository;

        public GetEffectiveModulesQueryHandler(
            ITenantSubscriptionRepository tenantSubscriptionRepository,
            IPlanModuleRepository planModuleRepository,
            ITenantModuleSubscriptionRepository tenantModuleSubscriptionRepository)
        {
            _tenantSubscriptionRepository = tenantSubscriptionRepository;
            _planModuleRepository = planModuleRepository;
            _tenantModuleSubscriptionRepository = tenantModuleSubscriptionRepository;
        }

        public async Task<GetEffectiveModulesResponse> Handle(GetEffectiveModulesQuery request, CancellationToken cancellationToken)
        {
            var subscription = await _tenantSubscriptionRepository.GetByTenantIdAsync(request.TenantId);

            var planModules = new List<Domain.Entities.Module>();
            if (subscription != null)
            {
                var enabledPlanModules = await _planModuleRepository.GetEnabledModulesAsync(subscription.PlanId);
                planModules = enabledPlanModules
                    .Where(pm => pm.Module != null)
                    .Select(pm => pm.Module!)
                    .ToList();
            }

            var purchasedModules = (await _tenantModuleSubscriptionRepository.GetActiveByTenantIdAsync(request.TenantId))
                .Where(tms => tms.Module != null)
                .Select(tms => tms.Module!)
                .ToList();

            var planCodes = planModules.Select(m => m.Code).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var purchasedCodes = purchasedModules.Select(m => m.Code).ToHashSet(StringComparer.OrdinalIgnoreCase);

            var allModules = new Dictionary<string, Domain.Entities.Module>(StringComparer.OrdinalIgnoreCase);
            foreach (var m in planModules.Concat(purchasedModules))
            {
                allModules[m.Code] = m;
            }

            var effectiveCodes = planCodes.Union(purchasedCodes);

            var result = effectiveCodes
                .Select(code =>
                {
                    var module = allModules[code];
                    var inPlan = planCodes.Contains(code);
                    var inPurchased = purchasedCodes.Contains(code);
                    var source = (inPlan, inPurchased) switch
                    {
                        (true, false) => "Plan",
                        (false, true) => "Purchased",
                        (true, true) => "PlanAndPurchased",
                        _ => "Unknown"
                    };
                    return new EffectiveModuleDto
                    {
                        Id = module.Id,
                        Code = module.Code,
                        Name = module.Name,
                        DisplayName = module.DisplayName,
                        Description = module.Description,
                        Source = source
                    };
                })
                .OrderBy(dto => dto.DisplayName)
                .ToList();

            return new GetEffectiveModulesResponse
            {
                Success = true,
                Data = result
            };
        }
    }
}
