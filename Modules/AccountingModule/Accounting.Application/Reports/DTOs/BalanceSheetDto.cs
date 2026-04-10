using System;
using System.Collections.Generic;

namespace Accounting.Application.Reports.DTOs
{
    /// <summary>Root Balance Sheet DTO returned to API callers.</summary>
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

        // ── Accounting equation check ───────────────────────────────────────────
        /// <summary>
        /// True when <see cref="TotalAssets"/> == <see cref="TotalLiabilities"/> + <see cref="TotalEquity"/>.
        /// A false value flags an out-of-balance ledger for the caller to handle.
        /// </summary>
        public bool IsBalanced { get; init; }

        /// <summary>The cut-off date this report was generated for.</summary>
        public DateTime AsOfDate { get; init; }
    }

    /// <summary>One account row within a Balance Sheet section.</summary>
    public class BalanceSheetItemDto
    {
        public int     AccountId   { get; init; }
        public string  AccountCode { get; init; } = null!;
        public string  AccountName { get; init; } = null!;
        public decimal Amount      { get; init; }
        public int?    CostCenterId   { get; init; }
        public string? CostCenterName { get; init; }
    }
}
