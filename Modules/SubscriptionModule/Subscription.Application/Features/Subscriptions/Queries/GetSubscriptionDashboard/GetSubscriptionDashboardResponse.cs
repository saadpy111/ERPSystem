using System.Collections.Generic;

namespace Subscription.Application.Features.Subscriptions.Queries.GetSubscriptionDashboard
{
    public class GetSubscriptionDashboardResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public SubscriptionDashboardDto? Data { get; set; }
    }

    public class SubscriptionDashboardDto
    {
        public string PlanName { get; set; } = string.Empty;
        public List<DashboardModuleDto> PlanModules { get; set; } = new();
        public List<DashboardModuleDto> EffectiveModules { get; set; } = new();
    }

    public class DashboardModuleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
