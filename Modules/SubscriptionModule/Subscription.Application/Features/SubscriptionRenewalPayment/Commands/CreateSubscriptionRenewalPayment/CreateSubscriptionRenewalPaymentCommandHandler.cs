using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using Subscription.Application.Contracts.Payment;
using Subscription.Application.Contracts.Persistence;
using Subscription.Domain.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.SubscriptionRenewalPayment.Commands.CreateSubscriptionRenewalPayment
{
    public sealed class CreateSubscriptionRenewalPaymentCommandHandler
        : IRequestHandler<CreateSubscriptionRenewalPaymentCommand, CreateSubscriptionRenewalPaymentResponse>
    {
        private readonly ITenantSubscriptionRepository _subscriptionRepository;
        private readonly ISubscriptionPlanRepository _planRepository;
        private readonly IPaymentInitiationService _paymentInitiationService;
        private readonly ILogger<CreateSubscriptionRenewalPaymentCommandHandler> _logger;

        public CreateSubscriptionRenewalPaymentCommandHandler(
            ITenantSubscriptionRepository subscriptionRepository,
            ISubscriptionPlanRepository planRepository,
            IPaymentInitiationService paymentInitiationService,
            ILogger<CreateSubscriptionRenewalPaymentCommandHandler> logger)
        {
            _subscriptionRepository = subscriptionRepository;
            _planRepository = planRepository;
            _paymentInitiationService = paymentInitiationService;
            _logger = logger;
        }

        public async Task<CreateSubscriptionRenewalPaymentResponse> Handle(
            CreateSubscriptionRenewalPaymentCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                // Load the tenant's subscription
                var subscription = await _subscriptionRepository.GetByTenantIdAsync(request.TenantId);
                if (subscription == null)
                    return Failure("No active subscription found for this tenant.");

                // Verify subscription status is renewable
                if (subscription.Status != Domain.Enums.SubscriptionStatus.Active &&
                    subscription.Status != Domain.Enums.SubscriptionStatus.Suspended)
                {
                    return Failure($"Subscription in status '{subscription.Status}' cannot be renewed.");
                }

                // Load the plan with prices
                var plan = await _planRepository.GetByIdAsync(subscription.PlanId);
                if (plan == null)
                    return Failure("Subscription plan not found.");

                // Resolve billing interval
                if (!Enum.TryParse<BillingInterval>(subscription.BillingCycle, ignoreCase: true, out var interval))
                    return Failure($"Cannot resolve billing interval from '{subscription.BillingCycle}'.");

                // Look up the price
                var price = plan.Prices?
                    .FirstOrDefault(p =>
                        p.IsActive &&
                        string.Equals(p.CurrencyCode, subscription.CurrencyCode, StringComparison.OrdinalIgnoreCase) &&
                        p.Interval == interval);

                if (price == null)
                    return Failure($"No active price found for plan '{plan.DisplayName}' in '{subscription.CurrencyCode}' with '{interval}' billing.");

                long amountCents = (long)Math.Round(price.Amount * 100m, MidpointRounding.AwayFromZero);

                // Initiate payment
                var result = await _paymentInitiationService.InitiateAsync(
                    new PaymentInitiationRequest(
                        UserId: request.UserId,
                        TenantId: request.TenantId,
                        Purpose: PaymentPurpose.SubscriptionRenewal,
                        TargetId: subscription.Id,
                        AmountCents: amountCents,
                        CurrencyCode: subscription.CurrencyCode,
                        Interval: interval,
                        CustomerFirstName: request.CustomerFirstName,
                        CustomerLastName: request.CustomerLastName,
                        CustomerEmail: request.CustomerEmail,
                        CustomerPhone: request.CustomerPhone,
                        Payload: null),
                    cancellationToken);

                if (!result.Success)
                    return Failure(result.Error ?? "Payment initiation failed.");

                return new CreateSubscriptionRenewalPaymentResponse
                {
                    Success = true,
                    PaymentId = result.PaymentId,
                    ClientSecret = result.ClientSecret,
                    CheckoutUrl = result.CheckoutUrl,
                    ReferenceId = result.ReferenceId,
                    PublicKey = result.PublicKey,
                    Status = result.Status
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating subscription renewal payment for tenant {TenantId}.", request.TenantId);
                return Failure("An error occurred while initiating the payment.");
            }
        }

        private static CreateSubscriptionRenewalPaymentResponse Failure(string error)
            => new() { Success = false, Error = error };
    }
}
