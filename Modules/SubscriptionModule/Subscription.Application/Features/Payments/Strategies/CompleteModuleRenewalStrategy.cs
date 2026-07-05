using MediatR;
using Subscription.Application.Contracts.Payment;
using Subscription.Application.Features.Modules.Commands.RenewModule;
using Subscription.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.Payments.Strategies
{
    public class CompleteModuleRenewalStrategy : IPaymentCompletionStrategy
    {
        private readonly IMediator _mediator;

        public PaymentPurpose Purpose => PaymentPurpose.ModuleRenewal;

        public CompleteModuleRenewalStrategy(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<PaymentCompletionResult> CompleteAsync(
            Subscription.Domain.Entities.Payment payment,
            Subscription.Domain.Entities.PaymentTransaction transaction,
            CancellationToken cancellationToken)
        {
            // For ModuleRenewal, TargetId is the ModuleCode
            var result = await _mediator.Send(
                new RenewModuleCommand
                {
                    TenantId = payment.TenantId,
                    ModuleCode = payment.TargetId
                },
                cancellationToken);

            return result.Success
                ? new PaymentCompletionResult(true)
                : new PaymentCompletionResult(false, result.Error);
        }
    }
}
