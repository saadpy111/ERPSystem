using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription.Infrastructure.Payment.Helper
{
    public sealed class PaymobApiException : Exception
    {
        public string? ResponseBody { get; }

        public int? StatusCode { get; }

        public PaymobApiException(string message)
            : base(message)
        {
        }

        public PaymobApiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public PaymobApiException(string message, string responseBody, int statusCode)
            : base(message)
        {
            ResponseBody = responseBody;
            StatusCode = statusCode;
        }
    }
}
