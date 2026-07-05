using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using Subscription.Application.Contracts.Payment;
using Subscription.Application.Contracts.Persistence;
using Subscription.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.ModuleRenewalPayment.Commands.CreateModuleRenewalPayment
{
    public sealed class CreateModuleRenewalPaymentCommandHandler
        : IRequestHandler<CreateModuleRenewalPaymentCommand, CreateModuleRenewalPaymentResponse>
    {
        private readonly ITenantModuleSubscriptionRepository _tenantModuleSubscriptionRepository;
        private readonly IModulePriceRepository _modulePriceRepository;
        private readonly IPaymentInitiationService _paymentInitiationService;
        private readonly ILogger<CreateModuleRenewalPaymentCommandHandler> _logger;

        public CreateModuleRenewalPaymentCommandHandler(
            ITenantModuleSubscriptionRepository tenantModuleSubscriptionRepository,
            IModulePriceRepository modulePriceRepository,
            IPaymentInitiationService paymentInitiationService,
            ILogger<CreateModuleRenewalPaymentCommandHandler> logger)
        {
            _tenantModuleSubscriptionRepository = tenantModuleSubscriptionRepository;
            _modulePriceRepository = modulePriceRepository;
            _paymentInitiationService = paymentInitiationService;
            _logger = logger;
        }

        public async Task<CreateModuleRenewalPaymentResponse> Handle(
            CreateModuleRenewalPaymentCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                // Verify tenant has an active module subscription
                var existing = await _tenantModuleSubscriptionRepository.FindActiveAsync(request.TenantId, request.ModuleCode);
                if (existing == null)
                    return Failure($"No active module subscription found for module '{request.ModuleCode}'.");

                // Look up the current active price
                var price = await _modulePriceRepository.GetActivePriceAsync(
                    existing.ModuleId, existing.CurrencyCode, existing.Interval);

                if (price == null)
                    return Failure($"No active price found for module '{request.ModuleCode}' in '{existing.CurrencyCode}' with '{existing.Interval}' billing.");

                long amountCents = (long)Math.Round(price.UnitPrice * 100m, MidpointRounding.AwayFromZero);

                // Initiate payment
                var result = await _paymentInitiationService.InitiateAsync(
                    new PaymentInitiationRequest(
                        UserId: request.UserId,
                        TenantId: request.TenantId,
                        Purpose: PaymentPurpose.ModuleRenewal,
                        TargetId: request.ModuleCode,
                        AmountCents: amountCents,
                        CurrencyCode: existing.CurrencyCode,
                        Interval: existing.Interval,
                        CustomerFirstName: request.CustomerFirstName,
                        CustomerLastName: request.CustomerLastName,
                        CustomerEmail: request.CustomerEmail,
                        CustomerPhone: request.CustomerPhone,
                        Payload: null),
                    cancellationToken);

                if (!result.Success)
                    return Failure(result.Error ?? "Payment initiation failed.");

                return new CreateModuleRenewalPaymentResponse
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
                _logger.LogError(ex, "Error creating module renewal payment for tenant {TenantId}, module {ModuleCode}.",
                    request.TenantId, request.ModuleCode);
                return Failure("An error occurred while initiating the payment.");
            }
        }

        private static CreateModuleRenewalPaymentResponse Failure(string error)
            => new() { Success = false, Error = error };
    }
}
