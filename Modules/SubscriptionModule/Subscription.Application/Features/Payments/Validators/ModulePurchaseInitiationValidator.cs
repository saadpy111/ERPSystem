using SharedKernel.Enums;
using Subscription.Application.Contracts.Payment;
using Subscription.Application.Contracts.Persistence;
using Subscription.Application.Features.Payments.Commands.InitiatePayment;
using Subscription.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.Payments.Validators
{
    public class ModulePurchaseInitiationValidator : IPaymentInitiationValidator
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IModulePriceRepository _modulePriceRepository;
        private readonly ITenantSubscriptionRepository _subscriptionRepository;
        private readonly IPlanModuleRepository _planModuleRepository;
        private readonly ITenantModuleSubscriptionRepository _tenantModuleSubscriptionRepository;

        public PaymentPurpose Purpose => PaymentPurpose.ModulePurchase;

        public ModulePurchaseInitiationValidator(
            IModuleRepository moduleRepository,
            IModulePriceRepository modulePriceRepository,
            ITenantSubscriptionRepository subscriptionRepository,
            IPlanModuleRepository planModuleRepository,
            ITenantModuleSubscriptionRepository tenantModuleSubscriptionRepository)
        {
            _moduleRepository = moduleRepository;
            _modulePriceRepository = modulePriceRepository;
            _subscriptionRepository = subscriptionRepository;
            _planModuleRepository = planModuleRepository;
            _tenantModuleSubscriptionRepository = tenantModuleSubscriptionRepository;
        }

        public async Task<InitiationValidationResult> ValidateAsync(InitiatePaymentCommand command, CancellationToken cancellationToken)
        {
            // 1. Verify module exists and is active
            var module = await _moduleRepository.GetByIdAsync(command.TargetId);
            if (module == null || !module.IsActive)
            {
                return new InitiationValidationResult(false, 0, "Module not found or not active.");
            }

            // 2. Verify tenant has an active subscription
            var subscription = await _subscriptionRepository.GetByTenantIdAsync(command.TenantId);
            if (subscription == null || 
                (subscription.Status != SubscriptionStatus.Active && subscription.Status != SubscriptionStatus.Trial))
            {
                return new InitiationValidationResult(false, 0, "Tenant must have an active subscription to purchase modules.");
            }

            // 3. Verify module not already included in current plan
            var isModuleInPlan = await _planModuleRepository.IsModuleEnabledInPlanAsync(subscription.PlanId, module.Code);
            if (isModuleInPlan)
            {
                return new InitiationValidationResult(false, 0, $"Module '{module.DisplayName}' is already included in your plan.");
            }

            // 4. Verify tenant does not already have an active purchase of this module
            var existingPurchase = await _tenantModuleSubscriptionRepository.FindActiveAsync(command.TenantId, module.Code);
            if (existingPurchase != null)
            {
                return new InitiationValidationResult(false, 0, $"Module '{module.DisplayName}' is already purchased and active.");
            }

            // 5. Load ModulePrice
            var price = await _modulePriceRepository.GetActivePriceAsync(module.Id, command.CurrencyCode, command.Interval);
            if (price == null)
            {
                return new InitiationValidationResult(false, 0, 
                    $"No active pricing found for module '{module.DisplayName}' in '{command.CurrencyCode}' with {command.Interval} billing.");
            }

            // Paymob amount in cents
            long amountCents = (long)Math.Round(price.UnitPrice * 100m, MidpointRounding.AwayFromZero);

            return new InitiationValidationResult(true, amountCents);
        }
    }
}
