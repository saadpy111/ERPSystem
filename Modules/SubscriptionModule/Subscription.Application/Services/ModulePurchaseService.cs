using SharedKernel.Enums;
using Subscription.Application.Contracts.Persistence;
using Subscription.Domain.Entities;
using Subscription.Domain.Enums;

namespace Subscription.Application.Services
{
    public class ModulePurchaseResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? SubscriptionId { get; set; }
    }

    public interface IModulePurchaseService
    {
        Task<ModulePurchaseResult> PurchaseModuleAsync(string tenantId, string moduleCode, string currencyCode, BillingInterval interval);
        Task<ModulePurchaseResult> CancelPurchasedModuleAsync(string tenantId, string moduleCode);
        Task<ModulePurchaseResult> RenewPurchasedModuleAsync(string tenantId, string moduleCode);
        Task<List<TenantModuleSubscription>> GetTenantPurchasedModulesAsync(string tenantId);
    }

    public class ModulePurchaseService : IModulePurchaseService
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IModulePriceRepository _modulePriceRepository;
        private readonly ITenantModuleSubscriptionRepository _tenantModuleSubscriptionRepository;
        private readonly IEffectiveModuleService _effectiveModuleService;
        private readonly IUnitOfWork _unitOfWork;

        public ModulePurchaseService(
            IModuleRepository moduleRepository,
            IModulePriceRepository modulePriceRepository,
            ITenantModuleSubscriptionRepository tenantModuleSubscriptionRepository,
            IEffectiveModuleService effectiveModuleService,
            IUnitOfWork unitOfWork)
        {
            _moduleRepository = moduleRepository;
            _modulePriceRepository = modulePriceRepository;
            _tenantModuleSubscriptionRepository = tenantModuleSubscriptionRepository;
            _effectiveModuleService = effectiveModuleService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ModulePurchaseResult> PurchaseModuleAsync(
            string tenantId, string moduleCode, string currencyCode, BillingInterval interval)
        {
            try
            {
                var modulesBefore = await _effectiveModuleService.GetEffectiveModulesAsync(tenantId);
                if (modulesBefore.Contains(moduleCode, StringComparer.OrdinalIgnoreCase))
                {
                    return new ModulePurchaseResult { Success = true };
                }

                var module = await _moduleRepository.GetByCodeAsync(moduleCode);
                if (module == null || !module.IsActive)
                {
                    return new ModulePurchaseResult { Success = false, Error = "Module not found or not available" };
                }

                var existing = await _tenantModuleSubscriptionRepository.FindActiveAsync(tenantId, moduleCode);
                if (existing != null)
                {
                    return new ModulePurchaseResult { Success = false, Error = "Module is already purchased" };
                }

                var price = await _modulePriceRepository.GetActivePriceAsync(module.Id, currencyCode, interval);
                if (price == null)
                {
                    return new ModulePurchaseResult
                    {
                        Success = false,
                        Error = $"No active pricing found for {moduleCode} in {currencyCode} with {interval} billing"
                    };
                }

                var now = DateTime.UtcNow;
                var periodEnd = interval switch
                {
                    BillingInterval.Monthly => now.AddMonths(1),
                    BillingInterval.Yearly => now.AddYears(1),
                    BillingInterval.Quarterly => now.AddMonths(3),
                    _ => now.AddMonths(1)
                };

                var subscription = new TenantModuleSubscription
                {
                    TenantId = tenantId,
                    ModuleId = module.Id,
                    UnitPrice = price.UnitPrice,
                    CurrencyCode = currencyCode.ToUpperInvariant(),
                    Interval = interval,
                    Status = ModuleSubscriptionStatus.Active,
                    StartDate = now,
                    EndDate = periodEnd,
                    AutoRenew = true,
                    CreatedAt = now,
                    UpdatedAt = now
                };

                await _tenantModuleSubscriptionRepository.CreateAsync(subscription);
                await _unitOfWork.SaveChangesAsync();

                return new ModulePurchaseResult
                {
                    Success = true,
                    SubscriptionId = subscription.Id
                };
            }
            catch (Exception ex)
            {
                return new ModulePurchaseResult
                {
                    Success = false,
                    Error = $"Failed to purchase module: {ex.Message}"
                };
            }
        }

        public async Task<ModulePurchaseResult> CancelPurchasedModuleAsync(string tenantId, string moduleCode)
        {
            try
            {
                var subscription = await _tenantModuleSubscriptionRepository.FindActiveAsync(tenantId, moduleCode);
                if (subscription == null)
                {
                    return new ModulePurchaseResult { Success = false, Error = "No active subscription found for this module" };
                }

                subscription.Status = ModuleSubscriptionStatus.Cancelled;
                subscription.EndDate = DateTime.UtcNow;
                subscription.UpdatedAt = DateTime.UtcNow;

                await _tenantModuleSubscriptionRepository.UpdateAsync(subscription);
                await _unitOfWork.SaveChangesAsync();

                return new ModulePurchaseResult { Success = true };
            }
            catch (Exception ex)
            {
                return new ModulePurchaseResult
                {
                    Success = false,
                    Error = $"Failed to cancel module: {ex.Message}"
                };
            }
        }

        public async Task<ModulePurchaseResult> RenewPurchasedModuleAsync(string tenantId, string moduleCode)
        {
            try
            {
                var subscription = await _tenantModuleSubscriptionRepository.FindActiveAsync(tenantId, moduleCode);
                if (subscription == null)
                {
                    return new ModulePurchaseResult { Success = false, Error = "No active subscription found for this module" };
                }

                var now = DateTime.UtcNow;
                subscription.EndDate = subscription.Interval switch
                {
                    BillingInterval.Monthly => now.AddMonths(1),
                    BillingInterval.Yearly => now.AddYears(1),
                    BillingInterval.Quarterly => now.AddMonths(3),
                    _ => now.AddMonths(1)
                };
                subscription.UpdatedAt = now;

                await _tenantModuleSubscriptionRepository.UpdateAsync(subscription);
                await _unitOfWork.SaveChangesAsync();

                return new ModulePurchaseResult { Success = true };
            }
            catch (Exception ex)
            {
                return new ModulePurchaseResult
                {
                    Success = false,
                    Error = $"Failed to renew module: {ex.Message}"
                };
            }
        }

        public async Task<List<TenantModuleSubscription>> GetTenantPurchasedModulesAsync(string tenantId)
        {
            return await _tenantModuleSubscriptionRepository.GetByTenantIdAsync(tenantId);
        }
    }
}
