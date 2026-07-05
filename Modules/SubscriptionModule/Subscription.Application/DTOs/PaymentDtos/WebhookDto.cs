using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Subscription.Application.DTOs.PaymentDtos
{

    public sealed class WebhookDto
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("obj")]
        public WebhookTransactionDto? Obj { get; set; }
    }

    public sealed class WebhookTransactionDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("pending")]
        public bool Pending { get; set; }

        [JsonPropertyName("amount_cents")]
        public long AmountCents { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("is_auth")]
        public bool IsAuth { get; set; }

        [JsonPropertyName("is_capture")]
        public bool IsCapture { get; set; }

        [JsonPropertyName("is_standalone_payment")]
        public bool IsStandalonePayment { get; set; }

        [JsonPropertyName("is_voided")]
        public bool IsVoided { get; set; }

        [JsonPropertyName("is_refunded")]
        public bool IsRefunded { get; set; }

        [JsonPropertyName("is_3d_secure")]
        public bool Is3DSecure { get; set; }

        [JsonPropertyName("integration_id")]
        public long IntegrationId { get; set; }

        [JsonPropertyName("profile_id")]
        public long ProfileId { get; set; }

        [JsonPropertyName("has_parent_transaction")]
        public bool HasParentTransaction { get; set; }

        [JsonPropertyName("order")]
        public WebhookOrderDto? Order { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonPropertyName("source_data")]
        public WebhookSourceDataDto? SourceData { get; set; }

        [JsonPropertyName("api_source")]
        public string? ApiSource { get; set; }

        [JsonPropertyName("merchant_commission")]
        public long MerchantCommission { get; set; }

        [JsonPropertyName("is_void")]
        public bool IsVoid { get; set; }

        [JsonPropertyName("is_refund")]
        public bool IsRefund { get; set; }

        [JsonPropertyName("data")]
        public WebhookGatewayDataDto? Data { get; set; }

        [JsonPropertyName("is_hidden")]
        public bool IsHidden { get; set; }

        [JsonPropertyName("payment_key_claims")]
        public WebhookPaymentKeyClaimsDto? PaymentKeyClaims { get; set; }

        [JsonPropertyName("error_occured")]
        public bool ErrorOccured { get; set; }

        [JsonPropertyName("is_live")]
        public bool IsLive { get; set; }

        [JsonPropertyName("refunded_amount_cents")]
        public long RefundedAmountCents { get; set; }

        [JsonPropertyName("source_id")]
        public long SourceId { get; set; }

        [JsonPropertyName("is_captured")]
        public bool IsCaptured { get; set; }

        [JsonPropertyName("captured_amount")]
        public long CapturedAmount { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("is_settled")]
        public bool IsSettled { get; set; }

        [JsonPropertyName("bill_balanced")]
        public bool BillBalanced { get; set; }

        [JsonPropertyName("is_bill")]
        public bool IsBill { get; set; }

        [JsonPropertyName("owner")]
        public long Owner { get; set; }
    }

    public sealed class WebhookOrderDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("delivery_needed")]
        public bool DeliveryNeeded { get; set; }

        [JsonPropertyName("amount_cents")]
        public long AmountCents { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("is_payment_locked")]
        public bool IsPaymentLocked { get; set; }

        [JsonPropertyName("merchant_order_id")]
        public string? MerchantOrderId { get; set; }

        [JsonPropertyName("paid_amount_cents")]
        public long PaidAmountCents { get; set; }

        [JsonPropertyName("data")]
        public Dictionary<string, object>? Data { get; set; }
    }

    public sealed class WebhookSourceDataDto
    {
        [JsonPropertyName("pan")]
        public string? Pan { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("sub_type")]
        public string? SubType { get; set; }

        [JsonPropertyName("tenure")]
        public object? Tenure { get; set; }
    }

    public sealed class WebhookGatewayDataDto
    {
        [JsonPropertyName("txn_response_code")]
        public string? TxnResponseCode { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("acq_response_code")]
        public string? AcqResponseCode { get; set; }

        [JsonPropertyName("merchant_txn_ref")]
        public string? MerchantTxnRef { get; set; }

        [JsonPropertyName("card_type")]
        public string? CardType { get; set; }

        [JsonPropertyName("card_num")]
        public string? CardNum { get; set; }

        [JsonPropertyName("secure_hash")]
        public string? SecureHash { get; set; }
    }

    public sealed class WebhookPaymentKeyClaimsDto
    {
        [JsonPropertyName("exp")]
        public long Exp { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("order_id")]
        public long OrderId { get; set; }

        [JsonPropertyName("amount_cents")]
        public long AmountCents { get; set; }

        [JsonPropertyName("integration_id")]
        public long IntegrationId { get; set; }

        [JsonPropertyName("extra")]
        public Dictionary<string, object>? Extra { get; set; }

        [JsonPropertyName("lock_order_when_paid")]
        public bool LockOrderWhenPaid { get; set; }

        [JsonPropertyName("single_payment_attempt")]
        public bool SinglePaymentAttempt { get; set; }
    }
}
