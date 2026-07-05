using Microsoft.Extensions.Options;
using Subscription.Application.Contracts.Infrastructure;
using Subscription.Application.DTOs.PaymentDtos;

namespace Subscription.Infrastructure.Payment.Helper
{
    public sealed class HmacService : IHmacService
    {
        private readonly PaymobOptions _options;

        public HmacService(IOptions<PaymobOptions> options)
        {
            _options = options.Value;
        }

        public bool VerifyHmac(string payload, string hmacSignature)
        {
            var calculatedHmac = HmacHelper.ComputeSha512(payload, _options.Hmac);
            return HmacHelper.SecureCompare(calculatedHmac, hmacSignature);
        }
    }
}
