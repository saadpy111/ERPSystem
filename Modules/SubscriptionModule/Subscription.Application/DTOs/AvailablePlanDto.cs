using Subscription.Domain.Enums;

namespace Subscription.Application.DTOs
{
    /// <summary>
    /// DTO for available subscription plans (public-facing).
    /// </summary>
    public class AvailablePlanDto
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Modules { get; set; } = new();
        public bool HasTrial { get; set; }
        public int TrialDays { get; set; }
        public List<PlanPricingDto> Pricing { get; set; } = new();
        public PlanLimitsDto Limits { get; set; } = new();
    }

    public class PlanPricingDto
    {
        public string Currency { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Interval { get; set; } = string.Empty; // Monthly, Yearly, Quarterly
        public string? Savings { get; set; } // e.g., "2 months free"
    }

    public class PlanLimitsDto
    {
        public int MaxUsers { get; set; }
        public string MaxStorage { get; set; } = string.Empty; // e.g., "10 GB"
        public int MaxProducts { get; set; }
        public int MaxMonthlyTransactions { get; set; }
    }
}
