using System;
using System.Collections.Generic;

namespace Accounting.Application.Reports.DTOs
{
    public class ExpenseAnalysisReportDto
    {
        public List<ExpenseAnalysisItemDto> Items { get; init; } = new();
        public decimal TotalExpense { get; init; }
        public DateTime FromDate { get; init; }
        public DateTime ToDate { get; init; }
    }

    public class ExpenseAnalysisItemDto
    {
        public int AccountId { get; init; }
        public string AccountCode { get; init; } = null!;
        public string AccountName { get; init; } = null!;
        public int? CostCenterId { get; init; }
        public string? CostCenterName { get; init; }
        public decimal Amount { get; init; }
        public decimal PercentageOfTotal { get; init; }
    }
}
