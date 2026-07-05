using SharedKernel.Enums;
using Subscription.Application.Contracts.Payment;
using Subscription.Application.Contracts.Persistence;
using Subscription.Application.Features.Payments.Commands.InitiatePayment;
using Subscription.Domain.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.Payments.Validators
{
    public class SubscriptionRenewalInitiationValidator : IPaymentInitiationValidator
    {
        private readonly ITenantSubscriptionRepository _subscriptionRepository;
        private readonly ISubscriptionPlanRepository _planRepository;

        public PaymentPurpose Purpose => PaymentPurpose.SubscriptionRenewal;

        public SubscriptionRenewalInitiationValidator(
            ITenantSubscriptionRepository subscriptionRepository,
            ISubscriptionPlanRepository planRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _planRepository = planRepository;
        }

        public async Task<InitiationValidationResult> ValidateAsync(InitiatePaymentCommand command, CancellationToken cancellationToken)
        {
            // 1. Load the tenant's subscription
            var subscription = await _subscriptionRepository.GetByTenantIdAsync(command.TenantId);
            if (subscription == null)
            {
                return new InitiationValidationResult(false, 0, "No active subscription found for this tenant.");
            }

            // 2. Verify the TargetId matches the actual subscription
            if (!string.Equals(subscription.Id, command.TargetId, StringComparison.OrdinalIgnoreCase))
            {
                return new InitiationValidationResult(false, 0, "Subscription ID mismatch.");
            }

            // 3. Verify subscription status is renewable
            if (subscription.Status != SubscriptionStatus.Active && subscription.Status != SubscriptionStatus.Suspended)
            {
                return new InitiationValidationResult(false, 0, $"Subscription in status '{subscription.Status}' cannot be renewed.");
            }

            // 4. Load the plan with prices
            var plan = await _planRepository.GetByIdAsync(subscription.PlanId);
            if (plan == null)
            {
                return new InitiationValidationResult(false, 0, "Subscription plan not found.");
            }

            // 5. Resolve billing interval from stored BillingCycle string
            if (!Enum.TryParse<BillingInterval>(subscription.BillingCycle, ignoreCase: true, out var interval))
            {
                return new InitiationValidationResult(false, 0, $"Cannot resolve billing interval from '{subscription.BillingCycle}'.");
            }

            // 6. Look up the price
            var price = plan.Prices
                .FirstOrDefault(p =>
                    p.IsActive &&
                    string.Equals(p.CurrencyCode, subscription.CurrencyCode, StringComparison.OrdinalIgnoreCase) &&
                    p.Interval == interval);

            if (price == null)
            {
                return new InitiationValidationResult(false, 0,
                    $"No active price found for plan '{plan.DisplayName}' in '{subscription.CurrencyCode}' with '{interval}' billing.");
            }

            long amountCents = (long)Math.Round(price.Amount * 100m, MidpointRounding.AwayFromZero);

            return new InitiationValidationResult(true, amountCents);
        }
    }
}
