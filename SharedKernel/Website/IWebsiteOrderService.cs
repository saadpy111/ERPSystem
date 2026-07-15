namespace SharedKernel.Website
{
    public interface IWebsiteOrderService
    {
        Task<WebsiteOrderCompletionResult> CompleteOrderPaymentAsync(
            CompleteOrderPaymentRequest request,
            CancellationToken cancellationToken = default);
    }

    public sealed record CompleteOrderPaymentRequest(
        string OrderId,
        long PaidAmountCents,
        string PaymentId);

    public sealed record WebsiteOrderCompletionResult(
        bool Success,
        string? Error);
}
