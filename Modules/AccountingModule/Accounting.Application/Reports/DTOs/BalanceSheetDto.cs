using Accounting.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Accounting.Application.Reports.DTOs
{
    public class BalanceSheetDto
    {
        // ── Classification buckets ──────────────────────────────────────────────
        public List<BalanceSheetItemDto> Assets      { get; init; } = new();
        public List<BalanceSheetItemDto> Liabilities { get; init; } = new();
        public List<BalanceSheetItemDto> Equity      { get; init; } = new();

        // ── Section totals ──────────────────────────────────────────────────────
        public decimal TotalAssets      { get; init; }
        public decimal TotalLiabilities { get; init; }
        public decimal TotalEquity      { get; init; }


        public bool IsBalanced { get; init; }

        public DateTime AsOfDate { get; init; }
    }

    public class BalanceSheetItemDto
    {
        public int     AccountId   { get; init; }
        public string  AccountCode { get; init; } = null!;
        public string  AccountName { get; init; } = null!;
        public AccountType AccountType { get; init; }
        public decimal Amount      { get; init; }
        public int?    CostCenterId   { get; init; }
        public string? CostCenterName { get; init; }
    }
}
