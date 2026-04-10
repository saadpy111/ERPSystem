using Accounting.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Accounting.Application.Features.Budgets.DTOs
{
    public class BudgetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int FiscalYearId { get; set; }
        public string FiscalYearName { get; set; } = null!;
        public BudgetStatus Status { get; set; }
        public bool EnforceBudgetControl { get; set; }
        public List<BudgetLineDto> Lines { get; set; } = new();
    }

    public class BudgetLineDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; } = null!;
        public int? CostCenterId { get; set; }
        public string? CostCenterName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PlannedAmount { get; set; }
    }

    public class BudgetVsActualDto
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; } = null!;
        public int? CostCenterId { get; set; }
        public string? CostCenterName { get; set; }
        public decimal PlannedAmount { get; set; }
        public decimal ActualAmount { get; set; }
        public decimal Variance { get; set; }
        public decimal VariancePercentage => PlannedAmount == 0 ? 0 : (Variance / PlannedAmount) * 100;
    }
}
