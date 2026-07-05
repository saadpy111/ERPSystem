using MediatR;
using Subscription.Application.Contracts.Payment;
using Subscription.Application.Features.Subscriptions.Commands.CompleteSubscriptionRenewal;
using Subscription.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.Payments.Strategies
{
    public class CompleteSubscriptionRenewalStrategy : IPaymentCompletionStrategy
    {
        private readonly IMediator _mediator;

        public PaymentPurpose Purpose => PaymentPurpose.SubscriptionRenewal;

        public CompleteSubscriptionRenewalStrategy(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<PaymentCompletionResult> CompleteAsync(
            Subscription.Domain.Entities.Payment payment,
            Subscription.Domain.Entities.PaymentTransaction transaction,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new CompleteSubscriptionRenewalCommand(
                    tenantId: payment.TenantId,
                    subscriptionId: payment.TargetId),
                cancellationToken);

            return result.Success
                ? new PaymentCompletionResult(true)
                : new PaymentCompletionResult(false, result.Error);
        }
    }
}
