using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SharedKernel.Multitenancy;
using Subscription.Application.Contracts.Infrastructure;
using Subscription.Application.Contracts.Payment;
using Subscription.Application.Contracts.Persistence;
using Subscription.Application.DTOs.PaymentDtos;
using Subscription.Domain.Entities;
using Subscription.Domain.Enums;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.Payments.Commands.ProcessWebhook
{
    public class ProcessWebhookCommandHandler : IRequestHandler<ProcessWebhookCommand, ProcessWebhookResponse>
    {
        private readonly IHmacService _hmacService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ProcessWebhookCommandHandler> _logger;
        private readonly ITenantProvider _tenantProvider;

        public ProcessWebhookCommandHandler(
            IHmacService hmacService,
            IPaymentRepository paymentRepository,
            IUnitOfWork unitOfWork,
            IServiceProvider serviceProvider,
            ILogger<ProcessWebhookCommandHandler> logger,
            ITenantProvider tenantProvider)
        {
            _hmacService = hmacService;
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _tenantProvider = tenantProvider;
        }

        public async Task<ProcessWebhookResponse> Handle(ProcessWebhookCommand request, CancellationToken cancellationToken)
        {
            var webhook = request.Webhook;

            // Step 1: Validate HMAC
            if (webhook.Obj == null)
            {
                _logger.LogWarning("Paymob webhook received with null transaction object.");
                return new ProcessWebhookResponse { Success = false, Message = "Null transaction object" };
            }

            var hmacPayload = BuildHmacPayload(webhook.Obj);
            if (!_hmacService.VerifyHmac(hmacPayload, request.Hmac))
            {
                _logger.LogWarning("HMAC verification failed for webhook callback.");
                // We return true/success response to Paymob to prevent infinite retries of bad HMAC signatures
                return new ProcessWebhookResponse { Success = false, Message = "HMAC verification failed" };
            }

            // Step 2: Parse webhook type (skip non-TRANSACTION events)
            if (!string.Equals(webhook.Type, "TRANSACTION", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("Skipping non-TRANSACTION event type: {Type}", webhook.Type);
                return new ProcessWebhookResponse { Success = true, Message = "Skipped non-TRANSACTION event" };
            }

            var obj = webhook.Obj;

            // Step 3: Extract SpecialReference (Payment.Id) from merchant_order_id
            if (obj.Order == null || string.IsNullOrWhiteSpace(obj.Order.MerchantOrderId))
            {
                _logger.LogWarning("Paymob webhook received without merchant_order_id.");
                return new ProcessWebhookResponse { Success = false, Message = "Missing merchant_order_id" };
            }

            var paymentId = obj.Order.MerchantOrderId;

            // Step 4: Load Payment by Id
            var payment = await _paymentRepository.GetByIdAsync(paymentId, cancellationToken);
            if (payment == null)
            {
                _logger.LogWarning("Payment not found for Id {PaymentId}.", paymentId);
                return new ProcessWebhookResponse { Success = false, Message = "Payment not found" };
            }

            // Step 5: Guard A - Payment Succeeded (Payment-level idempotency)
            if (payment.Status == PaymentStatus.Succeeded)
            {
                _logger.LogInformation("Payment {PaymentId} has already been marked Succeeded.", paymentId);
                return new ProcessWebhookResponse { Success = true, Message = "Payment already processed" };
            }

            // Step 6: Guard B - Transaction already exists (Transaction-level idempotency)
            var gatewayTxId = obj.Id.ToString();
            var transactionExists = await _paymentRepository.HasTransactionAsync(paymentId, gatewayTxId, cancellationToken);
            if (transactionExists)
            {
                _logger.LogInformation("Transaction {GatewayTxId} for Payment {PaymentId} already processed.", gatewayTxId, paymentId);
                return new ProcessWebhookResponse { Success = true, Message = "Transaction already processed" };
            }
            _tenantProvider.SetTenantId(request.Webhook.Obj.PaymentKeyClaims.Extra?["tenant_id"]?.ToString());
            // Step 7 & 8: Begin database transaction and write PaymentTransaction (even on failure, to track webhook history)
            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var paymentTransaction = new PaymentTransaction
                {
                    PaymentId = payment.Id,
                    GatewayTransactionId = gatewayTxId,
                    PaymentMethod = obj.SourceData?.Type ?? "Unknown",
                    AmountCents = obj.AmountCents,
                    Status = obj.Success ? PaymentTransactionStatus.Succeeded : PaymentTransactionStatus.Failed,
                    GatewayStatus = obj.Data?.Message ?? "No message",
                    ErrorCode = obj.Data?.TxnResponseCode,
                    ErrorMessage = obj.Data?.Message,
                    WebhookReceivedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };
                     paymentTransaction.Notes = JsonSerializer.Serialize(
                     new
                     {
                         Hmac = request.Hmac,
                         Webhook = request.Webhook
                     },
                     new JsonSerializerOptions
                     {
                         WriteIndented = true
                     });
                payment.Transactions.Add(paymentTransaction);

                // Step 9: Validate amount and success
                var paidAmountMatches = obj.AmountCents == payment.ExpectedAmountCents;
                if (!obj.Success || !paidAmountMatches)
                {
                    _logger.LogWarning("Payment {PaymentId} failed. Success={GatewaySuccess}, PaidCents={PaidCents}, ExpectedCents={ExpectedCents}",
                        paymentId, obj.Success, obj.AmountCents, payment.ExpectedAmountCents);

                    payment.Status = PaymentStatus.Failed;
                    await _paymentRepository.UpdateAsync(payment, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    return new ProcessWebhookResponse { Success = false, Message = "Payment failed at gateway or amount mismatch" };
                }

                // Step 10: Mark Payment as Succeeded
                payment.Status = PaymentStatus.Succeeded;
                await _paymentRepository.UpdateAsync(payment, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Step 11 & 12: Resolve completion strategy by Purpose and execute it
                var strategy = _serviceProvider.GetKeyedService<IPaymentCompletionStrategy>(payment.Purpose);
                if (strategy == null)
                {
                    _logger.LogError("No completion strategy registered for purpose: {Purpose}", payment.Purpose);
                    throw new InvalidOperationException($"Unsupported payment purpose for completion: {payment.Purpose}");
                }

                var completionResult = await strategy.CompleteAsync(payment, paymentTransaction, cancellationToken);
                if (!completionResult.Success)
                {
                    _logger.LogError("Completion strategy failed for Payment {PaymentId}: {Error}", paymentId, completionResult.Error);
                    throw new Exception($"Completion strategy failed: {completionResult.Error}");
                }

                // Step 13: Commit the database transaction
                await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation("Payment {PaymentId} and business operations completed successfully.", paymentId);

                return new ProcessWebhookResponse { Success = true, Message = "Payment processed successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transaction rolled back during webhook processing for Payment {PaymentId}.", paymentId);
                await transaction.RollbackAsync(cancellationToken);
                
                // Bubble up exception for MediatR / api to log and return appropriate status (so gateway can retry)
                throw;
            }
        }

        private static string BuildHmacPayload(WebhookTransactionDto obj)
        {
            var createdAtFormatted = obj.CreatedAt
                .ToString("yyyy-MM-ddTHH:mm:ss.fffffff")
                .TrimEnd('0');

            return string.Concat(
                obj.AmountCents.ToString(),
                createdAtFormatted,
                obj.Currency,
                obj.ErrorOccured.ToString().ToLowerInvariant(),
                obj.HasParentTransaction.ToString().ToLowerInvariant(),
                obj.Id.ToString(),
                obj.IntegrationId.ToString(),
                obj.Is3DSecure.ToString().ToLowerInvariant(),
                obj.IsAuth.ToString().ToLowerInvariant(),
                obj.IsCapture.ToString().ToLowerInvariant(),
                obj.IsRefunded.ToString().ToLowerInvariant(),
                obj.IsStandalonePayment.ToString().ToLowerInvariant(),
                obj.IsVoided.ToString().ToLowerInvariant(),
                obj.Order?.Id.ToString(),
                obj.Owner.ToString(),
                obj.Pending.ToString().ToLowerInvariant(),
                obj.SourceData?.Pan,
                obj.SourceData?.SubType,
                obj.SourceData?.Type,
                obj.Success.ToString().ToLowerInvariant());
        }
    }
}
