namespace Accounting.Application.Dashboard.DTOs
{
    public class BudgetUsageDto
    {
        public int BudgetId { get; init; }
        public string BudgetName { get; init; } = null!;
        public int? CostCenterId { get; init; }
        public string? CostCenterName { get; init; }
        public decimal PlannedAmount { get; init; }
        public decimal ActualAmount { get; init; }
        public decimal UsagePercentage { get; init; }
        public string Status { get; init; } = null!; // Normal / Warning / Exceeded
    }
}
