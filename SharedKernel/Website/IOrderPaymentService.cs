namespace SharedKernel.Website
{
    public interface IOrderPaymentService
    {
        Task<OrderPaymentInitiationResult> InitiateOrderPaymentAsync(
            InitiateOrderPaymentRequest request,
            CancellationToken cancellationToken = default);
    }

    public sealed record InitiateOrderPaymentRequest(
        string UserId,
        string TenantId,
        string OrderId,
        long AmountCents,
        string CurrencyCode,
        string CustomerFirstName,
        string CustomerLastName,
        string CustomerEmail,
        string CustomerPhone);

    public sealed record OrderPaymentInitiationResult(
        bool Success,
        string? Error,
        string? PaymentId,
        string? CheckoutUrl,
        string? ClientSecret,
        string? PublicKey,
        string? Status,
        bool IsReused);
}
