using Subscription.Domain.Enums;
using Subscription.Application.Features.Payments.Commands.InitiatePayment;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Contracts.Payment
{
    public interface IPaymentInitiationValidator
    {
        PaymentPurpose Purpose { get; }

        Task<InitiationValidationResult> ValidateAsync(
            InitiatePaymentCommand command,
            CancellationToken cancellationToken);
    }

    public sealed record InitiationValidationResult(
        bool IsValid,
        long ValidatedAmountCents,
        string? Error = null);
}
