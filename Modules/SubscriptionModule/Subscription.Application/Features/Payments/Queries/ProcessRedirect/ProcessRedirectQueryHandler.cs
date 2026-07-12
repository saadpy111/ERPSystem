using MediatR;
using Microsoft.Extensions.Logging;
using Subscription.Application.Contracts.Infrastructure;
using Subscription.Application.Contracts.Persistence;
using Subscription.Application.DTOs.PaymentDtos;
using Subscription.Domain.Enums;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.Payments.Queries.ProcessRedirect
{
    public sealed class ProcessRedirectQueryHandler
        : IRequestHandler<ProcessRedirectQuery, ProcessRedirectResponse>
    {
        private readonly IHmacService _hmacService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<ProcessRedirectQueryHandler> _logger;

        // Error codes matching PaymobErrorMessages in the Infrastructure layer.
        // Kept in Application to avoid a layer boundary violation.
        private static readonly Dictionary<string, string> ErrorMessages = new()
        {
            { "BLOCKED", "Process has been blocked from system." },
            { "B",       "Process has been blocked from system." },
            { "5",       "Balance is not enough." },
            { "F",       "Your card is not authorized with 3D secure." },
            { "7",       "Incorrect card expiration date." },
            { "2",       "Declined." },
            { "6051",    "Balance is not enough." },
            { "637",     "The OTP number was entered incorrectly." },
            { "11",      "Security checks are not passed by the system." }
        };

        public ProcessRedirectQueryHandler(
            IHmacService hmacService,
            IPaymentRepository paymentRepository,
            ILogger<ProcessRedirectQueryHandler> logger)
        {
            _hmacService = hmacService;
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        public async Task<ProcessRedirectResponse> Handle(
            ProcessRedirectQuery request,
            CancellationToken cancellationToken)
        {
            var callback = request.Callback;

            // Step 1: Validate HMAC
            // Reuses IHmacService — no hashing logic duplicated here.
            var hmacPayload = BuildHmacPayload(callback);
            if (!_hmacService.VerifyHmac(hmacPayload, request.Hmac))
            {
                _logger.LogWarning(
                    "HMAC verification failed for Paymob redirect callback. MerchantOrderId={MerchantOrderId}",
                    callback.MerchantOrderId);

                return new ProcessRedirectResponse
                {
                    Success = false,
                    Status  = PaymentStatus.Failed.ToString(),
                    Message = "Invalid signature. Request rejected."
                };
            }

            // Step 2: Validate merchant_order_id
            if (string.IsNullOrWhiteSpace(callback.MerchantOrderId))
            {
                _logger.LogWarning("Paymob redirect received without merchant_order_id.");
                return new ProcessRedirectResponse
                {
                    Success = false,
                    Status  = PaymentStatus.Failed.ToString(),
                    Message = "Missing payment reference."
                };
            }

            // Step 3: Load Payment — source of truth is always the database.
            // Reuses IPaymentRepository.GetByIdAsync — no lookup logic duplicated here.
            var payment = await _paymentRepository.GetByIdAsync(
                callback.MerchantOrderId, cancellationToken);

            if (payment == null)
            {
                _logger.LogWarning(
                    "Payment not found for redirect callback. MerchantOrderId={MerchantOrderId}",
                    callback.MerchantOrderId);

                return new ProcessRedirectResponse
                {
                    Success = false,
                    Status  = PaymentStatus.Failed.ToString(),
                    Message = "Payment record not found."
                };
            }

            // Step 4: Map database state → response.
            // Database is authoritative: if the redirect says success=true but the
            // database shows Pending (webhook not yet arrived) we return Pending,
            // not Succeeded. The frontend should poll until BusinessCompleted = true.
            var businessCompleted = payment.Status == PaymentStatus.Succeeded;
            var checkoutCompleted = callback.Success;

            var (responseSuccess, message) = payment.Status switch
            {
                PaymentStatus.Succeeded => (true,  "Payment completed successfully."),
                PaymentStatus.Failed    => (false, ResolveErrorMessage(callback.TxnResponseCode)),
                PaymentStatus.Pending   => (false, checkoutCompleted
                                                    ? "Payment is being processed. Please wait."
                                                    : ResolveErrorMessage(callback.TxnResponseCode)),
                _                       => (false, "Unknown payment state.")
            };

            _logger.LogInformation(
                "Redirect processed. PaymentId={PaymentId}, DbStatus={Status}, CheckoutCompleted={CheckoutCompleted}, BusinessCompleted={BusinessCompleted}",
                payment.Id, payment.Status, checkoutCompleted, businessCompleted);

            return new ProcessRedirectResponse
            {
                Success           = responseSuccess,
                Status            = payment.Status.ToString(),
                Message           = message,
                PaymentId         = payment.Id,
                Purpose           = payment.Purpose.ToString(),
                TargetId          = payment.TargetId,
                CheckoutCompleted = checkoutCompleted,
                BusinessCompleted = businessCompleted
            };
        }

        /// <summary>
        /// Builds the HMAC concatenation string from the redirect query parameters.
        /// Field order is identical to <c>ProcessWebhookCommandHandler.BuildHmacPayload</c>.
        /// <c>CreatedAt</c> is used as a raw string (not reformatted from DateTime) because
        /// the redirect delivers it as a pre-formatted query-string value.
        /// </summary>
        private static string BuildHmacPayload(PaymobRedirectQuery c) =>
            string.Concat(
                c.AmountCents.ToString(),
                c.CreatedAt,                                    
                c.Currency,
                c.ErrorOccured.ToString().ToLowerInvariant(),
                c.HasParentTransaction.ToString().ToLowerInvariant(),
                c.Id.ToString(),
                c.IntegrationId.ToString(),
                c.Is3DSecure.ToString().ToLowerInvariant(),
                c.IsAuth.ToString().ToLowerInvariant(),
                c.IsCapture.ToString().ToLowerInvariant(),
                c.IsRefunded.ToString().ToLowerInvariant(),
                c.IsStandalonePayment.ToString().ToLowerInvariant(),
                c.IsVoided.ToString().ToLowerInvariant(),
                c.Order.ToString(),
                c.Owner.ToString(),
                c.Pending.ToString().ToLowerInvariant(),
                c.SourceDataPan,
                c.SourceDataSubType,
                c.SourceDataType,
                c.Success.ToString().ToLowerInvariant());

        /// <summary>
        /// Resolves a Paymob transaction response code to a human-readable message.
        /// Mirrors the error table in <c>PaymobErrorMessages</c> (Infrastructure layer).
        /// Cannot reference Infrastructure directly due to the clean architecture boundary.
        /// </summary>
        private static string ResolveErrorMessage(string? code) =>
            code is not null && ErrorMessages.TryGetValue(code, out var message)
                ? message
                : "An error occurred while executing the operation.";
    }
}
