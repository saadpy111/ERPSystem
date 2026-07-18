using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

        [JsonPropertyName("data")]
        public WebhookGatewayDataDto? Data { get; set; }

        [JsonPropertyName("error_occured")]
        public bool ErrorOccured { get; set; }

        [JsonPropertyName("payment_key_claims")]
        public WebhookPaymentKeyClaimsDto? PaymentKeyClaims { get; set; }

        [JsonPropertyName("owner")]
        public long Owner { get; set; }
    }

    public sealed class WebhookOrderDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("merchant_order_id")]
        public string? MerchantOrderId { get; set; }
    }

    public sealed class WebhookSourceDataDto
    {
        [JsonPropertyName("pan")]
        public string? Pan { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("sub_type")]
        public string? SubType { get; set; }
    }

    public sealed class WebhookGatewayDataDto
    {
        [JsonPropertyName("txn_response_code")]
        public string? TxnResponseCode { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }

    public sealed class WebhookPaymentKeyClaimsDto
    {
        [JsonPropertyName("extra")]
        public Dictionary<string, object>? Extra { get; set; }
    }
}
