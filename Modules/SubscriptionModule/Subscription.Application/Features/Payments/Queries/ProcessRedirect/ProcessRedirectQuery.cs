using MediatR;
using Subscription.Application.DTOs.PaymentDtos;

namespace Subscription.Application.Features.Payments.Queries.ProcessRedirect
{
    /// <summary>
    /// CQRS Query that validates a Paymob browser redirect callback and returns
    /// the current payment state to the frontend.
    /// This query is read-only — it never executes business operations.
    /// </summary>
    public sealed class ProcessRedirectQuery : IRequest<ProcessRedirectResponse>
    {
        public string Hmac { get; }
        public PaymobRedirectQuery Callback { get; }

        public ProcessRedirectQuery(string hmac, PaymobRedirectQuery callback)
        {
            Hmac = hmac;
            Callback = callback;
        }
    }

    /// <summary>
    /// Frontend-friendly response describing the current payment state.
    /// </summary>
    public sealed class ProcessRedirectResponse
    {
        /// <summary>Whether the overall operation returned a meaningful result.</summary>
        public bool Success { get; set; }

        /// <summary>Database-authoritative payment status: Succeeded | Failed | Pending.</summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>Human-readable message for display in the frontend.</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>Our internal Payment.Id.</summary>
        public string PaymentId { get; set; } = string.Empty;

        /// <summary>Payment purpose (e.g. ModulePurchase, SubscriptionRenewal).</summary>
        public string Purpose { get; set; } = string.Empty;

        /// <summary>ID of the business entity being paid for (module, plan, etc.).</summary>
        public string TargetId { get; set; } = string.Empty;

        /// <summary>
        /// True when Paymob confirmed success on the redirect (gateway checkout completed).
        /// Reflects the redirect <c>success</c> query param — not yet the database.
        /// </summary>
        public bool CheckoutCompleted { get; set; }

        /// <summary>
        /// True when Payment.Status == Succeeded, meaning the webhook already ran
        /// and all business operations completed. False when still Pending or Failed.
        /// </summary>
        public bool BusinessCompleted { get; set; }
    }
}
