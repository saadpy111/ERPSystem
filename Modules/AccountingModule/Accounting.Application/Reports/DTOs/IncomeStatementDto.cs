using Accounting.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Accounting.Application.Reports.DTOs
{
    public class IncomeStatementDto
    {
        public List<IncomeStatementItemDto> Revenues { get; init; } = new();
        public List<IncomeStatementItemDto> Expenses { get; init; } = new();

        public decimal TotalRevenue { get; init; }
        public decimal TotalExpenses { get; init; }
        public decimal NetProfit { get; init; }

        public DateTime FromDate { get; init; }
        public DateTime ToDate { get; init; }
    }

    public class IncomeStatementItemDto
    {
        public int AccountId { get; init; }
        public string AccountCode { get; init; } = null!;
        public string AccountName { get; set; } = null!;
        public AccountType AccountType { get; init; }
        public decimal Amount { get; set; }
        public int? CostCenterId { get; init; }
        public string? CostCenterName { get; init; }
    }
}
