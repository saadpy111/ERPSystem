using MediatR;

namespace Subscription.Application.Features.Subscriptions.Commands.CompleteSubscriptionRenewal
{
    public class CompleteSubscriptionRenewalCommand : IRequest<CompleteSubscriptionRenewalResponse>
    {
        public string TenantId { get; }
        public string SubscriptionId { get; }

        public CompleteSubscriptionRenewalCommand(string tenantId, string subscriptionId)
        {
            TenantId = tenantId;
            SubscriptionId = subscriptionId;
        }
    }

    public class CompleteSubscriptionRenewalResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
