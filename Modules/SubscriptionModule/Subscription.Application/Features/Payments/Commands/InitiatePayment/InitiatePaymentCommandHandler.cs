using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedKernel.Multitenancy;
using Subscription.Application.Contracts.Infrastructure;
using Subscription.Application.Contracts.Payment;
using Subscription.Application.Contracts.Persistence;
using Subscription.Application.DTOs.PaymentDtos;
using Subscription.Domain.Entities;
using Subscription.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.Payments.Commands.InitiatePayment
{
    public class InitiatePaymentCommandHandler : IRequestHandler<InitiatePaymentCommand, InitiatePaymentResponse>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymobPaymentService _paymobPaymentService;
        private readonly IServiceProvider _serviceProvider;
        private readonly PaymobOptions _paymobOptions;
        private readonly ILogger<InitiatePaymentCommandHandler> _logger;
        private readonly ITenantProvider _tenantProvider;

        public InitiatePaymentCommandHandler(
            IPaymentRepository paymentRepository,
            IUnitOfWork unitOfWork,
            IPaymobPaymentService paymobPaymentService,
            IServiceProvider serviceProvider,
            IOptions<PaymobOptions> paymobOptions,
            ILogger<InitiatePaymentCommandHandler> logger,
            ITenantProvider tenantProvider)
        {
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
            _paymobPaymentService = paymobPaymentService;
            _serviceProvider = serviceProvider;
            _paymobOptions = paymobOptions.Value;
            _logger = logger;
            _tenantProvider = tenantProvider;
        }

        public async Task<InitiatePaymentResponse> Handle(InitiatePaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                request.TenantId = _tenantProvider.GetTenantId()??"";
                // Resolve Keyed Validator
                var validator = _serviceProvider.GetKeyedService<IPaymentInitiationValidator>(request.Purpose);
                if (validator == null)
                {
                    _logger.LogError("No validator registered for payment purpose: {Purpose}", request.Purpose);
                    return new InitiatePaymentResponse
                    {
                        Success = false,
                        Error = $"Unsupported payment purpose: {request.Purpose}"
                    };
                }

                // Run pre-payment validation
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return new InitiatePaymentResponse
                    {
                        Success = false,
                        Error = validationResult.Error ?? "Validation failed"
                    };
                }

                // Check for existing pending payment (Idempotency)
                var existingPendingPayment = await _paymentRepository.FindPendingAsync(
                    request.TenantId,
                    request.Purpose,
                    request.TargetId,
                    cancellationToken);

                if (existingPendingPayment != null)
                {
                    _logger.LogInformation("Reusing existing pending payment {PaymentId} for tenant {TenantId}.", 
                        existingPendingPayment.Id, request.TenantId);

                    return new InitiatePaymentResponse
                    {
                        Success = true,
                        PaymentId = existingPendingPayment.Id,
                        CheckoutUrl = existingPendingPayment.CheckoutUrl,
                        ClientSecret = existingPendingPayment.ClientSecret,
                        IsReused = true,
                        Status = existingPendingPayment.Status.ToString(),
                        PublicKey = _paymobOptions.PublicKey
                    };
                }

                // Create new Payment entity
                var payment = new Payment
                {
                    TenantId = request.TenantId,
                    Purpose = request.Purpose,
                    TargetId = request.TargetId,
                    ExpectedAmountCents = validationResult.ValidatedAmountCents,
                    CurrencyCode = request.CurrencyCode,
                    Interval = request.Interval,
                    Status = PaymentStatus.Pending,
                    CustomerFirstName = request.CustomerFirstName,
                    CustomerLastName = request.CustomerLastName,
                    CustomerEmail = request.CustomerEmail,
                    CustomerPhone = request.CustomerPhone,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddSeconds(_paymobOptions.ExpirationSeconds)
                };

                await _paymentRepository.CreateAsync(payment, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Call Paymob Payment Intention
                decimal amountDecimal = validationResult.ValidatedAmountCents / 100m;
                var paymobCommand = new CreatePaymentIntentionCommand(
                    PaymentId: payment.Id,
                    TenantId: request.TenantId,
                    Amount: amountDecimal,
                    CurrencyCode: request.CurrencyCode,
                    ItemName: $"{request.Purpose}-{request.TargetId}",
                    ItemDescription: $"Payment for {request.Purpose} with target {request.TargetId} for tenant {request.TenantId}",
                    CustomerFirstName: request.CustomerFirstName,
                    CustomerLastName: request.CustomerLastName,
                    CustomerEmail: request.CustomerEmail,
                    CustomerPhone: request.CustomerPhone
                );

                var paymobResult = await _paymobPaymentService.CreatePaymentIntentionAsync(paymobCommand, cancellationToken);

                // Update payment with gateway order info and secrets
                payment.GatewayOrderId = paymobResult.IntentionOrderId;
                payment.ClientSecret = paymobResult.ClientSecret;
                payment.CheckoutUrl = paymobResult.CheckoutUrl;
                
                await _paymentRepository.UpdateAsync(payment, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new InitiatePaymentResponse
                {
                    Success = true,
                    PaymentId = payment.Id,
                    ClientSecret = paymobResult.ClientSecret,
                    CheckoutUrl = paymobResult.CheckoutUrl,
                    ReferenceId = paymobResult.ReferenceId,
                    PublicKey = _paymobOptions.PublicKey,
                    Status = payment.Status.ToString(),
                    IsReused = false
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while initiating payment for tenant {TenantId}.", request.TenantId);
                return new InitiatePaymentResponse
                {
                    Success = false,
                    Error = "An error occurred while initiating the payment intention."
                };
            }
        }
    }
}
