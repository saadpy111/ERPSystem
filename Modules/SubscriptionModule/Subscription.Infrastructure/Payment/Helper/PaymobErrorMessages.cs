using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription.Infrastructure.Payment.Helper
{

    public static class PaymobErrorMessages
    {
        private static readonly Dictionary<string, string> Messages = new()
        {
            { "BLOCKED", "Process has been blocked from system." },
            { "B", "Process has been blocked from system." },
            { "5", "Balance is not enough." },
            { "F", "Your card is not authorized with 3D secure." },
            { "7", "Incorrect card expiration date." },
            { "2", "Declined." },
            { "6051", "Balance is not enough." },
            { "637", "The OTP number was entered incorrectly." },
            { "11", "Security checks are not passed by the system." }
        };

        public static string Resolve(string? code)
        {
            if (code is not null && Messages.TryGetValue(code, out var message))
            {
                return message;
            }

            return "An error occurred while executing the operation.";
        }
    }
}
