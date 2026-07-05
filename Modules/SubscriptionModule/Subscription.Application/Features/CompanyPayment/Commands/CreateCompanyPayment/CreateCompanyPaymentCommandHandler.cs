using Identity.Application.Contracts.Persistence;
using Identity.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using Subscription.Application.Contracts.Payment;
using Subscription.Application.Contracts.Persistence;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.CompanyPayment.Commands.CreateCompanyPayment
{
    public sealed class CreateCompanyPaymentCommandHandler
        : IRequestHandler<CreateCompanyPaymentCommand, CreateCompanyPaymentResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly ISubscriptionPlanRepository _planRepository;
        private readonly IPaymentInitiationService _paymentInitiationService;
        private readonly ILogger<CreateCompanyPaymentCommandHandler> _logger;

        public CreateCompanyPaymentCommandHandler(
            IAuthRepository authRepository,
            ITenantRepository tenantRepository,
            ISubscriptionPlanRepository planRepository,
            IPaymentInitiationService paymentInitiationService,
            ILogger<CreateCompanyPaymentCommandHandler> logger)
        {
            _authRepository = authRepository;
            _tenantRepository = tenantRepository;
            _planRepository = planRepository;
            _paymentInitiationService = paymentInitiationService;
            _logger = logger;
        }

        public async Task<CreateCompanyPaymentResponse> Handle(
            CreateCompanyPaymentCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                // Validate user exists and is in PendingTenant state
                var user = await _authRepository.FindByIdAsync(request.UserId);
                if (user == null)
                    return Failure("User not found.");

                if (user.TenantId != null)
                    return Failure("User already belongs to a company.");

                if (user.State != UserTenantState.PendingTenant)
                    return Failure("User is not in pending tenant state.");

                // Validate company code is unique
                if (await _tenantRepository.ExistsAsync(request.CompanyCode))
                    return Failure("Company code already exists.");

                // Validate plan and calculate amount
                var plan = await _planRepository.GetByCodeAsync(request.PlanCode);
                if (plan == null || !plan.IsActive)
                    return Failure("Plan not found or not active.");

                var price = plan.Prices?.FirstOrDefault(p =>
                    p.IsActive &&
                    p.CurrencyCode.Equals(request.CurrencyCode, StringComparison.OrdinalIgnoreCase) &&
                    p.Interval == request.Interval);
               
                if (price == null)
                    return Failure($"No active pricing found for plan '{request.PlanCode}' in '{request.CurrencyCode}' with {request.Interval} billing.");

                long amountCents = (long)Math.Round(price.Amount * 100m, MidpointRounding.AwayFromZero);

                // Build payload for completion strategy
                var payload = JsonSerializer.Serialize(new
                {
                    request.CompanyName,
                    request.CompanyCode,
                    request.UserId,
                    request.PlanCode,
                    request.CurrencyCode,
                    request.Interval
                });

                // Initiate payment
                var result = await _paymentInitiationService.InitiateAsync(
                    new PaymentInitiationRequest(
                        UserId: request.UserId,
                        TenantId: null,
                        Purpose: Domain.Enums.PaymentPurpose.CreateCompany,
                        TargetId: price.PlanId,
                        AmountCents: amountCents,
                        CurrencyCode: request.CurrencyCode,
                        Interval: request.Interval,
                        CustomerFirstName: request.CustomerFirstName,
                        CustomerLastName: request.CustomerLastName,
                        CustomerEmail: request.CustomerEmail,
                        CustomerPhone: request.CustomerPhone,
                        Payload: payload),
                    cancellationToken);

                if (!result.Success)
                    return Failure(result.Error ?? "Payment initiation failed.");

                return new CreateCompanyPaymentResponse
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
                _logger.LogError(ex, "Error creating company payment for user {UserId}.", request.UserId);
                return Failure("An error occurred while initiating the payment.");
            }
        }

        private static CreateCompanyPaymentResponse Failure(string error)
            => new() { Success = false, Error = error };
    }
}
