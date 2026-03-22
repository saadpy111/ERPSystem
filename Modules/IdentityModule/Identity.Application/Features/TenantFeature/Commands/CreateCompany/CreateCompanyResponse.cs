using System;

namespace Identity.Application.Features.TenantFeature.Commands.CreateCompany
{
    public class CreateCompanyResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? TenantId { get; set; }
        public string? TenantName { get; set; }
        public string? NewToken { get; set; } // JWT with tenant context
        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();

        // NEW: Subscription details
        public string? SubscriptionPlanName { get; set; }
        public bool IsTrial { get; set; }
        public DateTime? TrialEndsAt { get; set; }
    }
}
