using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Subscription.Application.DTOs.PaymentDtos
{
    public sealed class PaymentIntentionRequest
    {
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = "EGP";

        [JsonPropertyName("payment_methods")]
        public List<object> PaymentMethods { get; set; } = new();

        [JsonPropertyName("items")]
        public List<PaymentIntentionItem> Items { get; set; } = new();

        [JsonPropertyName("billing_data")]
        public BillingData BillingData { get; set; } = new();

        [JsonPropertyName("extras")]
        public Extras? Extras { get; set; }

        [JsonPropertyName("special_reference")]
        public string SpecialReference { get; set; } = string.Empty;

        [JsonPropertyName("expiration")]
        public int? Expiration { get; set; }

        [JsonPropertyName("notification_url")]
        public string? NotificationUrl { get; set; }

        [JsonPropertyName("redirection_url")]
        public string? RedirectionUrl { get; set; }
    }

    public sealed class PaymentIntentionItem
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; } = 1;
    }

    public sealed class BillingData
    {
        [JsonPropertyName("apartment")]
        public string Apartment { get; set; } = "NA";

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("floor")]
        public string Floor { get; set; } = "NA";

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("street")]
        public string Street { get; set; } = "NA";

        [JsonPropertyName("building")]
        public string Building { get; set; } = "NA";

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [JsonPropertyName("shipping_method")]
        public string ShippingMethod { get; set; } = "NA";

        [JsonPropertyName("postal_code")]
        public string PostalCode { get; set; } = "NA";

        [JsonPropertyName("city")]
        public string City { get; set; } = "NA";

        [JsonPropertyName("country")]
        public string Country { get; set; } = "NA";

        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("state")]
        public string State { get; set; } = "NA";
    }

    public sealed class Extras
    {
        [JsonPropertyName("subscription_id")]
        public int? SubscriptionId { get; set; }

        [JsonPropertyName("tenant_id")]
        public string? TenantId { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object>? AdditionalData { get; set; }
    }
}
