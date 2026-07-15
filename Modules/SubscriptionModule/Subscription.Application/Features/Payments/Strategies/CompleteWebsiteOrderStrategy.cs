using Microsoft.Extensions.Logging;
using SharedKernel.Website;
using Subscription.Application.Contracts.Payment;
using Subscription.Domain.Entities;
using Subscription.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.Payments.Strategies
{
    public class CompleteWebsiteOrderStrategy : IPaymentCompletionStrategy
    {
        private readonly IWebsiteOrderService _websiteOrderService;
        private readonly ILogger<CompleteWebsiteOrderStrategy> _logger;

        public PaymentPurpose Purpose => PaymentPurpose.WebsiteOrder;

        public CompleteWebsiteOrderStrategy(
            IWebsiteOrderService websiteOrderService,
            ILogger<CompleteWebsiteOrderStrategy> logger)
        {
            _websiteOrderService = websiteOrderService;
            _logger = logger;
        }

        public async Task<PaymentCompletionResult> CompleteAsync(
            Payment payment,
            PaymentTransaction transaction,
            CancellationToken cancellationToken)
        {
            try
            {
                var request = new CompleteOrderPaymentRequest(
                    OrderId: payment.TargetId,
                    PaidAmountCents: transaction.AmountCents,
                    PaymentId: payment.Id);

                var result = await _websiteOrderService.CompleteOrderPaymentAsync(request, cancellationToken);

                if (!result.Success)
                {
                    _logger.LogError(
                        "Website order completion failed for Payment {PaymentId}, Order {OrderId}: {Error}",
                        payment.Id, payment.TargetId, result.Error);
                    return new PaymentCompletionResult(false, result.Error);
                }

                _logger.LogInformation(
                    "Website order {OrderId} completed from Payment {PaymentId}.",
                    payment.TargetId, payment.Id);

                return new PaymentCompletionResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "CompleteWebsiteOrderStrategy failed for Payment {PaymentId}, Order {OrderId}.",
                    payment.Id, payment.TargetId);
                return new PaymentCompletionResult(false, $"Order completion failed: {ex.Message}");
            }
        }
    }
}
