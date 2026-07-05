using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using Subscription.Application.Contracts.Payment;
using Subscription.Application.Contracts.Persistence;
using Subscription.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.ModulePayment.Commands.CreateModulePurchasePayment
{
    public sealed class CreateModulePurchasePaymentCommandHandler
        : IRequestHandler<CreateModulePurchasePaymentCommand, CreateModulePurchasePaymentResponse>
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IModulePriceRepository _modulePriceRepository;
        private readonly ITenantSubscriptionRepository _subscriptionRepository;
        private readonly IPlanModuleRepository _planModuleRepository;
        private readonly ITenantModuleSubscriptionRepository _tenantModuleSubscriptionRepository;
        private readonly IPaymentInitiationService _paymentInitiationService;
        private readonly ILogger<CreateModulePurchasePaymentCommandHandler> _logger;

        public CreateModulePurchasePaymentCommandHandler(
            IModuleRepository moduleRepository,
            IModulePriceRepository modulePriceRepository,
            ITenantSubscriptionRepository subscriptionRepository,
            IPlanModuleRepository planModuleRepository,
            ITenantModuleSubscriptionRepository tenantModuleSubscriptionRepository,
            IPaymentInitiationService paymentInitiationService,
            ILogger<CreateModulePurchasePaymentCommandHandler> logger)
        {
            _moduleRepository = moduleRepository;
            _modulePriceRepository = modulePriceRepository;
            _subscriptionRepository = subscriptionRepository;
            _planModuleRepository = planModuleRepository;
            _tenantModuleSubscriptionRepository = tenantModuleSubscriptionRepository;
            _paymentInitiationService = paymentInitiationService;
            _logger = logger;
        }

        public async Task<CreateModulePurchasePaymentResponse> Handle(
            CreateModulePurchasePaymentCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                // Verify module exists and is active
                var module = await _moduleRepository.GetByCodeAsync(request.ModuleCode);
                if (module == null || !module.IsActive)
                    return Failure("Module not found or not active.");

                // Verify tenant has an active subscription
                var subscription = await _subscriptionRepository.GetByTenantIdAsync(request.TenantId);
                if (subscription == null ||
                    (subscription.Status != Domain.Enums.SubscriptionStatus.Active &&
                     subscription.Status != Domain.Enums.SubscriptionStatus.Trial))
                {
                    return Failure("Tenant must have an active subscription to purchase modules.");
                }

                // Verify module not already included in current plan
                var isModuleInPlan = await _planModuleRepository.IsModuleEnabledInPlanAsync(subscription.PlanId, module.Code);
                if (isModuleInPlan)
                    return Failure($"Module '{module.DisplayName}' is already included in your plan.");

                // Verify tenant does not already have an active purchase
                var existingPurchase = await _tenantModuleSubscriptionRepository.FindActiveAsync(request.TenantId, module.Code);
                if (existingPurchase != null)
                    return Failure($"Module '{module.DisplayName}' is already purchased and active.");

                // Load pricing
                var price = await _modulePriceRepository.GetActivePriceAsync(module.Id, request.CurrencyCode, request.Interval);
                if (price == null)
                    return Failure($"No active pricing found for module '{module.DisplayName}' in '{request.CurrencyCode}' with {request.Interval} billing.");

                long amountCents = (long)Math.Round(price.UnitPrice * 100m, MidpointRounding.AwayFromZero);

                // Initiate payment
                var result = await _paymentInitiationService.InitiateAsync(
                    new PaymentInitiationRequest(
                        UserId: request.UserId,
                        TenantId: request.TenantId,
                        Purpose: PaymentPurpose.ModulePurchase,
                        TargetId: module.Id,
                        AmountCents: amountCents,
                        CurrencyCode: request.CurrencyCode,
                        Interval: request.Interval,
                        CustomerFirstName: request.CustomerFirstName,
                        CustomerLastName: request.CustomerLastName,
                        CustomerEmail: request.CustomerEmail,
                        CustomerPhone: request.CustomerPhone,
                        Payload: null),
                    cancellationToken);

                if (!result.Success)
                    return Failure(result.Error ?? "Payment initiation failed.");

                return new CreateModulePurchasePaymentResponse
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
                _logger.LogError(ex, "Error creating module purchase payment for tenant {TenantId}, module {ModuleCode}.",
                    request.TenantId, request.ModuleCode);
                return Failure("An error occurred while initiating the payment.");
            }
        }

        private static CreateModulePurchasePaymentResponse Failure(string error)
            => new() { Success = false, Error = error };
    }
}
