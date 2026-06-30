using MediatR;
using SharedKernel.Events;
using SharedKernel.Enums;
using Subscription.Application.Contracts.Persistence;
using Subscription.Application.Services;
using Subscription.Domain.Entities;
using Subscription.Domain.Enums;

namespace Subscription.Application.Features.Modules.Commands.PurchaseModule
{
    public class PurchaseModuleCommandHandler : IRequestHandler<PurchaseModuleCommand, PurchaseModuleResponse>
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IModulePriceRepository _modulePriceRepository;
        private readonly ITenantModuleSubscriptionRepository _tenantModuleSubscriptionRepository;
        private readonly IEffectiveModuleService _effectiveModuleService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public PurchaseModuleCommandHandler(
            IModuleRepository moduleRepository,
            IModulePriceRepository modulePriceRepository,
            ITenantModuleSubscriptionRepository tenantModuleSubscriptionRepository,
            IEffectiveModuleService effectiveModuleService,
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _moduleRepository = moduleRepository;
            _modulePriceRepository = modulePriceRepository;
            _tenantModuleSubscriptionRepository = tenantModuleSubscriptionRepository;
            _effectiveModuleService = effectiveModuleService;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<PurchaseModuleResponse> Handle(PurchaseModuleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var modulesBefore = await _effectiveModuleService.GetEffectiveModulesAsync(request.TenantId);
                if (modulesBefore.Contains(request.ModuleCode, StringComparer.OrdinalIgnoreCase))
                {
                    return new PurchaseModuleResponse { Success = true };
                }

                var module = await _moduleRepository.GetByCodeAsync(request.ModuleCode);
                if (module == null || !module.IsActive)
                {
                    return new PurchaseModuleResponse { Success = false, Error = "Module not found or not available" };
                }

                var existing = await _tenantModuleSubscriptionRepository.FindActiveAsync(request.TenantId, request.ModuleCode);
                if (existing != null)
                {
                    return new PurchaseModuleResponse { Success = false, Error = "Module is already purchased" };
                }

                var price = await _modulePriceRepository.GetActivePriceAsync(module.Id, request.CurrencyCode, request.Interval);
                if (price == null)
                {
                    return new PurchaseModuleResponse
                    {
                        Success = false,
                        Error = $"No active pricing found for {request.ModuleCode} in {request.CurrencyCode} with {request.Interval} billing"
                    };
                }

                var now = DateTime.UtcNow;
                var periodEnd = request.Interval switch
                {
                    BillingInterval.Monthly => now.AddMonths(1),
                    BillingInterval.Yearly => now.AddYears(1),
                    BillingInterval.Quarterly => now.AddMonths(3),
                    _ => now.AddMonths(1)
                };

                var subscription = new TenantModuleSubscription
                {
                    TenantId = request.TenantId,
                    ModuleId = module.Id,
                    UnitPrice = price.UnitPrice,
                    CurrencyCode = request.CurrencyCode.ToUpperInvariant(),
                    Interval = request.Interval,
                    Status = ModuleSubscriptionStatus.Active,
                    StartDate = now,
                    EndDate = periodEnd,
                    AutoRenew = true,
                    CreatedAt = now,
                    UpdatedAt = now
                };

                await _tenantModuleSubscriptionRepository.CreateAsync(subscription);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var modulesAfter = await _effectiveModuleService.GetEffectiveModulesAsync(request.TenantId);
                if (!modulesBefore.ToHashSet().SetEquals(modulesAfter))
                {
                    await _mediator.Publish(new TenantModulesChangedNotification { TenantId = request.TenantId }, cancellationToken);
                }

                return new PurchaseModuleResponse
                {
                    Success = true,
                    SubscriptionId = subscription.Id
                };
            }
            catch (Exception ex)
            {
                return new PurchaseModuleResponse
                {
                    Success = false,
                    Error = $"Failed to purchase module: {ex.Message}"
                };
            }
        }
    }
}
