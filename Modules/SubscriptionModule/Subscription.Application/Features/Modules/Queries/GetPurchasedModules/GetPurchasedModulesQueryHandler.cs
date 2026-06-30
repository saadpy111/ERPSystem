using MediatR;
using Subscription.Application.Contracts.Persistence;
using Subscription.Application.DTOs;

namespace Subscription.Application.Features.Modules.Queries.GetPurchasedModules
{
    public class GetPurchasedModulesQueryHandler : IRequestHandler<GetPurchasedModulesQuery, GetPurchasedModulesResponse>
    {
        private readonly ITenantModuleSubscriptionRepository _tenantModuleSubscriptionRepository;

        public GetPurchasedModulesQueryHandler(
            ITenantModuleSubscriptionRepository tenantModuleSubscriptionRepository)
        {
            _tenantModuleSubscriptionRepository = tenantModuleSubscriptionRepository;
        }

        public async Task<GetPurchasedModulesResponse> Handle(GetPurchasedModulesQuery request, CancellationToken cancellationToken)
        {
            var subscriptions = await _tenantModuleSubscriptionRepository.GetByTenantIdAsync(request.TenantId);

            var result = subscriptions.Select(s => new PurchasedModuleDto
            {
                Id = s.Id,
                ModuleCode = s.Module.Code,
                ModuleName = s.Module.Name,
                UnitPrice = s.UnitPrice,
                CurrencyCode = s.CurrencyCode,
                Interval = s.Interval.ToString(),
                Status = s.Status.ToString(),
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                AutoRenew = s.AutoRenew
            }).ToList();

            return new GetPurchasedModulesResponse
            {
                Success = true,
                Data = result
            };
        }
    }
}
