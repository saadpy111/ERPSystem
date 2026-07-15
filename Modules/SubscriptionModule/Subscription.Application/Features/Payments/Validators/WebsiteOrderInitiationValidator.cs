using Subscription.Application.Contracts.Payment;
using Subscription.Application.Features.Payments.Commands.InitiatePayment;
using Subscription.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.Payments.Validators
{
    public class WebsiteOrderInitiationValidator : IPaymentInitiationValidator
    {
        public PaymentPurpose Purpose => PaymentPurpose.WebsiteOrder;

        public Task<InitiationValidationResult> ValidateAsync(InitiatePaymentCommand command, CancellationToken cancellationToken)
        {
            var amountCents = command.ExpectedAmountCents;
            return Task.FromResult(new InitiationValidationResult(true, amountCents));
        }
    }
}
