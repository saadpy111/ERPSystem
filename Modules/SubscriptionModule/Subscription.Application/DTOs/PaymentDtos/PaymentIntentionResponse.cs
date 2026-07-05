using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Subscription.Application.DTOs.PaymentDtos
{

    public sealed class PaymentIntentionResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("intention_order_id")]
        public long IntentionOrderId { get; set; }

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; } = string.Empty;

        [JsonPropertyName("confirmed")]
        public bool Confirmed { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("created")]
        public string Created { get; set; } = string.Empty;

        [JsonPropertyName("special_reference")]
        public string? SpecialReference { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("payment_keys")]
        public List<PaymentKeyDto>? PaymentKeys { get; set; }

        [JsonPropertyName("payment_methods")]
        public List<PaymentMethodResponseDto>? PaymentMethods { get; set; }

        [JsonPropertyName("extras")]
        public Extras? Extras { get; set; }

        [JsonPropertyName("card_detail")]
        public string? CardDetail { get; set; }

        [JsonPropertyName("card_tokens")]
        public List<object>? CardTokens { get; set; }

        [JsonPropertyName("object")]
        public string? ObjectType { get; set; }
    }

    public sealed class PaymentKeyDto
    {
        [JsonPropertyName("integration")]
        public long Integration { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;

        [JsonPropertyName("gateway_type")]
        public string? GatewayType { get; set; }

        [JsonPropertyName("iframe_id")]
        public long? IframeId { get; set; }

        [JsonPropertyName("order")]
        public string? Order { get; set; }
    }

    public sealed class PaymentMethodResponseDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("method_type")]
        public string? MethodType { get; set; }

        [JsonPropertyName("live")]
        public bool Live { get; set; }
    }
}
