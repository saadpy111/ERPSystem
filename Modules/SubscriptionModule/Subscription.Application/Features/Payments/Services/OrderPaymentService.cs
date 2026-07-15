using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Website;
using Subscription.Application.Contracts.Payment;
using Subscription.Application.Features.Payments.Commands.InitiatePayment;
using Subscription.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.Payments.Services
{
    public class OrderPaymentService : IOrderPaymentService
    {
        private readonly IPaymentInitiationService _paymentInitiationService;
        private readonly ILogger<OrderPaymentService> _logger;

        public OrderPaymentService(
            IPaymentInitiationService paymentInitiationService,
            ILogger<OrderPaymentService> logger)
        {
            _paymentInitiationService = paymentInitiationService;
            _logger = logger;
        }

        public async Task<OrderPaymentInitiationResult> InitiateOrderPaymentAsync(
            InitiateOrderPaymentRequest request,
            CancellationToken cancellationToken)
        {
            var initiationRequest = new PaymentInitiationRequest(
                UserId: request.UserId,
                TenantId: request.TenantId,
                Purpose: PaymentPurpose.WebsiteOrder,
                TargetId: request.OrderId,
                AmountCents: request.AmountCents,
                CurrencyCode: request.CurrencyCode,
                Interval: BillingInterval.OneTime,
                CustomerFirstName: request.CustomerFirstName,
                CustomerLastName: request.CustomerLastName,
                CustomerEmail: request.CustomerEmail,
                CustomerPhone: request.CustomerPhone);

            var result = await _paymentInitiationService.InitiateAsync(initiationRequest, cancellationToken);

            return new OrderPaymentInitiationResult(
                Success: result.Success,
                Error: result.Error,
                PaymentId: result.PaymentId,
                CheckoutUrl: result.CheckoutUrl,
                ClientSecret: result.ClientSecret,
                PublicKey: result.PublicKey,
                Status: result.Status,
                IsReused: result.IsReused);
        }
    }
}
