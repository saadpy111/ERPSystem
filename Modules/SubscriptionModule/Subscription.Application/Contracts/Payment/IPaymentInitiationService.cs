using SharedKernel.Enums;
using Subscription.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Contracts.Payment
{
    public interface IPaymentInitiationService
    {
        Task<PaymentInitiationResult> InitiateAsync(
            PaymentInitiationRequest request,
            CancellationToken cancellationToken = default);
    }

    public sealed record PaymentInitiationRequest(
        string UserId,
        string? TenantId,
        PaymentPurpose Purpose,
        string TargetId,
        long AmountCents,
        string CurrencyCode,
        BillingInterval Interval,
        string CustomerFirstName,
        string CustomerLastName,
        string CustomerEmail,
        string CustomerPhone,
        string? Payload = null);

    public sealed record PaymentInitiationResult(
        bool Success,
        string? Error,
        string? PaymentId,
        string? ClientSecret,
        string? CheckoutUrl,
        string? ReferenceId,
        string? PublicKey,
        string? Status,
        bool IsReused);
}
