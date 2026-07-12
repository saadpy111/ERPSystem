using Microsoft.AspNetCore.Mvc;

namespace Subscription.Application.DTOs.PaymentDtos
{
    /// <summary>
    /// Flat query-string parameters sent by Paymob when it redirects the browser
    /// back to our redirect URL after a payment attempt.
    /// Contains only the fields that Paymob currently sends and that our implementation needs.
    /// </summary>
    public sealed class PaymobRedirectQuery
    {
        // ── Identity ────────────────────────────────────────────────────────────

        /// <summary>Paymob gateway transaction ID. Used in HMAC payload.</summary>
        [FromQuery(Name = "id")]
        public long Id { get; set; }

        /// <summary>
        /// Our internal Payment.Id (set as <c>merchant_order_id</c> when creating the intention).
        /// Used to load the Payment record from the repository.
        /// </summary>
        [FromQuery(Name = "merchant_order_id")]
        public string MerchantOrderId { get; set; } = string.Empty;

        /// <summary>Paymob gateway order ID. Used in HMAC payload only.</summary>
        [FromQuery(Name = "order")]
        public long Order { get; set; }

        // ── HMAC-required fields ────────────────────────────────────────────────

        [FromQuery(Name = "amount_cents")]
        public long AmountCents { get; set; }

        /// <summary>
        /// Raw string value as sent by Paymob (e.g. "2024-01-15T10:30:00.123456").
        /// Concatenated as-is into the HMAC payload — must not be reformatted.
        /// </summary>
        [FromQuery(Name = "created_at")]
        public string CreatedAt { get; set; } = string.Empty;

        [FromQuery(Name = "currency")]
        public string Currency { get; set; } = string.Empty;

        [FromQuery(Name = "error_occured")]
        public bool ErrorOccured { get; set; }

        [FromQuery(Name = "has_parent_transaction")]
        public bool HasParentTransaction { get; set; }

        [FromQuery(Name = "integration_id")]
        public long IntegrationId { get; set; }

        [FromQuery(Name = "is_3d_secure")]
        public bool Is3DSecure { get; set; }

        [FromQuery(Name = "is_auth")]
        public bool IsAuth { get; set; }

        [FromQuery(Name = "is_capture")]
        public bool IsCapture { get; set; }

        [FromQuery(Name = "is_refunded")]
        public bool IsRefunded { get; set; }

        [FromQuery(Name = "is_standalone_payment")]
        public bool IsStandalonePayment { get; set; }

        [FromQuery(Name = "is_voided")]
        public bool IsVoided { get; set; }

        [FromQuery(Name = "owner")]
        public long Owner { get; set; }

        [FromQuery(Name = "pending")]
        public bool Pending { get; set; }

        [FromQuery(Name = "source_data.pan")]
        public string? SourceDataPan { get; set; }

        [FromQuery(Name = "source_data.sub_type")]
        public string? SourceDataSubType { get; set; }

        [FromQuery(Name = "source_data.type")]
        public string? SourceDataType { get; set; }

        [FromQuery(Name = "success")]
        public bool Success { get; set; }

        // ── Error information ───────────────────────────────────────────────────

        /// <summary>
        /// Paymob transaction response code. Used to resolve a human-readable
        /// error message when <see cref="Success"/> is <c>false</c>.
        /// </summary>
        [FromQuery(Name = "txn_response_code")]
        public string? TxnResponseCode { get; set; }

        // ── HMAC ────────────────────────────────────────────────────────────────

        [FromQuery(Name = "hmac")]
        public string Hmac { get; set; } = string.Empty;
    }
}
