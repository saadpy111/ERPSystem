using MediatR;

namespace Subscription.Application.Features.Subscriptions.Queries.GetSubscriptionDashboard
{
    public class GetSubscriptionDashboardQuery : IRequest<GetSubscriptionDashboardResponse>
    {
        public string TenantId { get; set; } = string.Empty;
    }
}
