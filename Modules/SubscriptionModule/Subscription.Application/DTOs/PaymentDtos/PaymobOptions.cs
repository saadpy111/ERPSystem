using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription.Application.DTOs.PaymentDtos
{

    public sealed class PaymobOptions
    {
        public const string SectionName = "Paymob";

        [Required]
        public string SecretKey { get; set; } = string.Empty;

        [Required]
        public string PublicKey { get; set; } = string.Empty;

        [Required]
        public string IntegrationId { get; set; } = string.Empty;

        public string Currency { get; set; } = "EGP";

        [Required]
        public string Hmac { get; set; } = string.Empty;

        public string BaseUrl { get; set; } = "https://accept.paymob.com";

        public string? NotificationUrl { get; set; }

        public string? RedirectionUrl { get; set; }

        public int ExpirationSeconds { get; set; } = 3600;
    }
}
