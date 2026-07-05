using MediatR;
using SharedKernel.Enums;
using SharedKernel.Subscription;
using Subscription.Application.Contracts.Persistence;
using Subscription.Application.Services;
using Subscription.Domain.Entities;
using Subscription.Domain.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.Modules.Commands.FulfillModulePurchase
{
    public class FulfillModulePurchaseCommandHandler : IRequestHandler<FulfillModulePurchaseCommand, FulfillModulePurchaseResponse>
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IModulePriceRepository _modulePriceRepository;
        private readonly ITenantModuleSubscriptionRepository _tenantModuleSubscriptionRepository;
        private readonly IEffectiveModuleService _effectiveModuleService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantRoleProvisioningService _tenantRoleProvisioningService;

        public FulfillModulePurchaseCommandHandler(
            IModuleRepository moduleRepository,
            IModulePriceRepository modulePriceRepository,
            ITenantModuleSubscriptionRepository tenantModuleSubscriptionRepository,
            IEffectiveModuleService effectiveModuleService,
            IUnitOfWork unitOfWork,
            ITenantRoleProvisioningService tenantRoleProvisioningService)
        {
            _moduleRepository = moduleRepository;
            _modulePriceRepository = modulePriceRepository;
            _tenantModuleSubscriptionRepository = tenantModuleSubscriptionRepository;
            _effectiveModuleService = effectiveModuleService;
            _unitOfWork = unitOfWork;
            _tenantRoleProvisioningService = tenantRoleProvisioningService;
        }

        public async Task<FulfillModulePurchaseResponse> Handle(FulfillModulePurchaseCommand request, CancellationToken cancellationToken)
        {
            // 1. Load Module by ModuleId
            var module = await _moduleRepository.GetByIdAsync(request.ModuleId);
            if (module == null || !module.IsActive)
            {
                return new FulfillModulePurchaseResponse { Success = false, Error = "Module not found or not active." };
            }

            // 2. Guard: verify no active subscription already exists for this tenant & module (Idempotency)
            var existing = await _tenantModuleSubscriptionRepository.FindActiveAsync(request.TenantId, module.Code);
            if (existing != null)
            {
                return new FulfillModulePurchaseResponse
                {
                    Success = true,
                    SubscriptionId = existing.Id
                };
            }

            // 3. Load active price
            var price = await _modulePriceRepository.GetActivePriceAsync(request.ModuleId, request.CurrencyCode, request.Interval);
            if (price == null)
            {
                return new FulfillModulePurchaseResponse
                {
                    Success = false,
                    Error = $"Active price not found for Module: {module.Code}, Currency: {request.CurrencyCode}, Interval: {request.Interval}"
                };
            }

            // 4. Calculate EndDate
            var now = DateTime.UtcNow;
            var endDate = request.Interval switch
            {
                BillingInterval.Monthly => now.AddMonths(1),
                BillingInterval.Yearly => now.AddYears(1),
                BillingInterval.Quarterly => now.AddMonths(3),
                _ => now.AddMonths(1)
            };

            // 5. Create active subscription
            var subscription = new TenantModuleSubscription
            {
                TenantId = request.TenantId,
                ModuleId = request.ModuleId,
                UnitPrice = price.UnitPrice,
                CurrencyCode = request.CurrencyCode.ToUpperInvariant(),
                Interval = request.Interval,
                Status = ModuleSubscriptionStatus.Active,
                StartDate = now,
                EndDate = endDate,
                AutoRenew = true,
                CreatedAt = now,
                UpdatedAt = now
            };

            await _tenantModuleSubscriptionRepository.CreateAsync(subscription);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 6. Role Provisioning & Permission Sync
            var modulesAfter = await _effectiveModuleService.GetEffectiveModulesAsync(request.TenantId);
            var provisionResult = await _tenantRoleProvisioningService.ProvisionRolesAsync(request.TenantId, modulesAfter);

            if (!provisionResult.Success)
            {
                return new FulfillModulePurchaseResponse { Success = false, Error = provisionResult.Error };
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new FulfillModulePurchaseResponse
            {
                Success = true,
                SubscriptionId = subscription.Id
            };
        }
    }
}
