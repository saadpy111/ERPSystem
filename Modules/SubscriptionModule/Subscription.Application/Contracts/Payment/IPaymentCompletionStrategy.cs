using Subscription.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Contracts.Payment
{
    public interface IPaymentCompletionStrategy
    {
        PaymentPurpose Purpose { get; }

        Task<PaymentCompletionResult> CompleteAsync(
            Subscription.Domain.Entities.Payment payment,
            Subscription.Domain.Entities.PaymentTransaction transaction,
            CancellationToken cancellationToken);
    }

    public sealed record PaymentCompletionResult(bool Success, string? Error = null);
}
