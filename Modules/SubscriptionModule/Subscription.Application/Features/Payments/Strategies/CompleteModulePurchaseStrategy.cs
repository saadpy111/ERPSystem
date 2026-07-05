using MediatR;
using Subscription.Application.Contracts.Payment;
using Subscription.Application.Features.Modules.Commands.FulfillModulePurchase;
using Subscription.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.Payments.Strategies
{
    public class CompleteModulePurchaseStrategy : IPaymentCompletionStrategy
    {
        private readonly IMediator _mediator;

        public PaymentPurpose Purpose => PaymentPurpose.ModulePurchase;

        public CompleteModulePurchaseStrategy(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<PaymentCompletionResult> CompleteAsync(
            Subscription.Domain.Entities.Payment payment,
            Subscription.Domain.Entities.PaymentTransaction transaction,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new FulfillModulePurchaseCommand(
                    tenantId: payment.TenantId,
                    moduleId: payment.TargetId,
                    currencyCode: payment.CurrencyCode,
                    interval: payment.Interval),
                cancellationToken);

            return result.Success
                ? new PaymentCompletionResult(true)
                : new PaymentCompletionResult(false, result.Error);
        }
    }
}
