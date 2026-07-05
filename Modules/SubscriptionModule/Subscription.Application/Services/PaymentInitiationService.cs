using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Subscription.Application.Contracts.Infrastructure;
using Subscription.Application.Contracts.Payment;
using Subscription.Application.Contracts.Persistence;
using Subscription.Application.DTOs.PaymentDtos;
using Subscription.Domain.Entities;
using Subscription.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Services
{
    public sealed class PaymentInitiationService : IPaymentInitiationService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymobPaymentService _paymobPaymentService;
        private readonly PaymobOptions _paymobOptions;
        private readonly ILogger<PaymentInitiationService> _logger;

        public PaymentInitiationService(
            IPaymentRepository paymentRepository,
            IUnitOfWork unitOfWork,
            IPaymobPaymentService paymobPaymentService,
            IOptions<PaymobOptions> paymobOptions,
            ILogger<PaymentInitiationService> logger)
        {
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
            _paymobPaymentService = paymobPaymentService;
            _paymobOptions = paymobOptions.Value;
            _logger = logger;
        }

        public async Task<PaymentInitiationResult> InitiateAsync(
            PaymentInitiationRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                // Idempotency: check for existing pending payment
                Payment? existingPendingPayment;
                if (!string.IsNullOrEmpty(request.TenantId))
                {
                    existingPendingPayment = await _paymentRepository.FindPendingAsync(
                        request.TenantId,
                        request.Purpose,
                        request.TargetId,
                        cancellationToken);
                }
                else
                {
                    existingPendingPayment = await _paymentRepository.FindPendingByUserAsync(
                        request.UserId,
                        request.Purpose,
                        request.TargetId,
                        cancellationToken);
                }

                if (existingPendingPayment != null)
                {
                    _logger.LogInformation(
                        "Reusing existing pending payment {PaymentId} for user {UserId}.",
                        existingPendingPayment.Id, request.UserId);

                    return new PaymentInitiationResult(
                        Success: true,
                        Error: null,
                        PaymentId: existingPendingPayment.Id,
                        ClientSecret: existingPendingPayment.ClientSecret,
                        CheckoutUrl: existingPendingPayment.CheckoutUrl,
                        ReferenceId: existingPendingPayment.Id,
                        PublicKey: _paymobOptions.PublicKey,
                        Status: existingPendingPayment.Status.ToString(),
                        IsReused: true);
                }

                // Create new Payment entity
                var payment = new Payment
                {
                    UserId = request.UserId,
                    TenantId = request.TenantId,
                    Purpose = request.Purpose,
                    TargetId = request.TargetId,
                    ExpectedAmountCents = request.AmountCents,
                    CurrencyCode = request.CurrencyCode,
                    Interval = request.Interval,
                    Status = PaymentStatus.Pending,
                    CustomerFirstName = request.CustomerFirstName,
                    CustomerLastName = request.CustomerLastName,
                    CustomerEmail = request.CustomerEmail,
                    CustomerPhone = request.CustomerPhone,
                    Payload = request.Payload,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddSeconds(_paymobOptions.ExpirationSeconds)
                };

                await _paymentRepository.CreateAsync(payment, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Call Paymob Payment Intention
                decimal amountDecimal = request.AmountCents / 100m;
                var paymobCommand = new CreatePaymentIntentionCommand(
                    PaymentId: payment.Id,
                    TenantId: request.TenantId ?? request.UserId,
                    Amount: amountDecimal,
                    CurrencyCode: request.CurrencyCode,
                    ItemName: $"{request.Purpose}-{request.TargetId}",
                    ItemDescription: $"Payment for {request.Purpose} with target {request.TargetId}",
                    CustomerFirstName: request.CustomerFirstName,
                    CustomerLastName: request.CustomerLastName,
                    CustomerEmail: request.CustomerEmail,
                    CustomerPhone: request.CustomerPhone
                );

                var paymobResult = await _paymobPaymentService.CreatePaymentIntentionAsync(paymobCommand, cancellationToken);

                // Update payment with gateway info
                payment.GatewayOrderId = paymobResult.IntentionOrderId;
                payment.ClientSecret = paymobResult.ClientSecret;
                payment.CheckoutUrl = paymobResult.CheckoutUrl;

                await _paymentRepository.UpdateAsync(payment, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Payment {PaymentId} initiated for user {UserId}, purpose {Purpose}.",
                    payment.Id, request.UserId, request.Purpose);

                return new PaymentInitiationResult(
                    Success: true,
                    Error: null,
                    PaymentId: payment.Id,
                    ClientSecret: paymobResult.ClientSecret,
                    CheckoutUrl: paymobResult.CheckoutUrl,
                    ReferenceId: paymobResult.ReferenceId,
                    PublicKey: _paymobOptions.PublicKey,
                    Status: payment.Status.ToString(),
                    IsReused: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initiating payment for user {UserId}, purpose {Purpose}.", request.UserId, request.Purpose);
                return new PaymentInitiationResult(
                    Success: false,
                    Error: "An error occurred while initiating the payment.",
                    PaymentId: null,
                    ClientSecret: null,
                    CheckoutUrl: null,
                    ReferenceId: null,
                    PublicKey: null,
                    Status: null,
                    IsReused: false);
            }
        }
    }
}
