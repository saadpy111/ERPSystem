using Subscription.Application.DTOs.PaymentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription.Application.Contracts.Infrastructure
{

    public interface IPaymobPaymentService
    {
        Task<PaymentIntentionResult> CreatePaymentIntentionAsync(
            CreatePaymentIntentionCommand command,
            CancellationToken cancellationToken = default);
    }
}
