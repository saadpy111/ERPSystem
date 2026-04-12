using System;
using System.Collections.Generic;

namespace Accounting.Application.Reports.DTOs
{
    public class ProfitabilityReportDto
    {
        public List<ProfitabilityItemDto> Items { get; init; } = new();
        public decimal TotalRevenue { get; init; }
        public decimal TotalExpenses { get; init; }
        public decimal TotalProfit { get; init; }
        public DateTime FromDate { get; init; }
        public DateTime ToDate { get; init; }
    }

    public class ProfitabilityItemDto
    {
        public int? CostCenterId { get; init; }
        public string? CostCenterName { get; init; }
        public decimal Revenue { get; init; }
        public decimal Expenses { get; init; }
        public decimal Profit { get; init; }
        public decimal ProfitMargin { get; init; } 
    }
}
