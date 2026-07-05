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
    public class ModuleRenewalInitiationValidator : IPaymentInitiationValidator
    {
        private readonly ITenantModuleSubscriptionRepository _tenantModuleSubscriptionRepository;
        private readonly IModulePriceRepository _modulePriceRepository;

        public PaymentPurpose Purpose => PaymentPurpose.ModuleRenewal;

        public ModuleRenewalInitiationValidator(
            ITenantModuleSubscriptionRepository tenantModuleSubscriptionRepository,
            IModulePriceRepository modulePriceRepository)
        {
            _tenantModuleSubscriptionRepository = tenantModuleSubscriptionRepository;
            _modulePriceRepository = modulePriceRepository;
        }

        public async Task<InitiationValidationResult> ValidateAsync(InitiatePaymentCommand command, CancellationToken cancellationToken)
        {
            // For ModuleRenewal, TargetId is ModuleCode
            var moduleCode = command.TargetId;

            // 1. Verify tenant has an active module subscription 
            var existing = await _tenantModuleSubscriptionRepository.FindActiveAsync(command.TenantId, moduleCode);
            if (existing == null)
            {
                return new InitiationValidationResult(false, 0, $"No active module subscription found for module '{moduleCode}'.");
            }

            // 2. Look up the current active price using the subscription's stored currency and interval
            var price = await _modulePriceRepository.GetActivePriceAsync(
                existing.ModuleId,
                existing.CurrencyCode,
                existing.Interval);

            if (price == null)
            {
                return new InitiationValidationResult(false, 0,
                    $"No active price found for module '{moduleCode}' in '{existing.CurrencyCode}' with '{existing.Interval}' billing.");
            }

            long amountCents = (long)Math.Round(price.UnitPrice * 100m, MidpointRounding.AwayFromZero);

            return new InitiationValidationResult(true, amountCents);
        }
    }
}
