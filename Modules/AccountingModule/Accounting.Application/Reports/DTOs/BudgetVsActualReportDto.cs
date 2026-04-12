using System.Collections.Generic;

namespace Accounting.Application.Reports.DTOs
{
    public class BudgetVsActualReportDto
    {
        public List<BudgetVsActualItemDto> Items { get; init; } = new();
    }

    public class BudgetVsActualItemDto
    {
        public int AccountId { get; init; }
        public string AccountCode { get; init; } = null!;
        public string AccountName { get; init; } = null!;
        public int? CostCenterId { get; init; }
        public string? CostCenterName { get; init; }
        public decimal PlannedAmount { get; init; }
        public decimal ActualAmount { get; init; }
        public decimal Variance { get; init; } // Planned - Actual (or vice-versa depending on expense/revenue)
        public decimal UsagePercentage { get; init; }
    }
}
