using System;
using System.Collections.Generic;

namespace Accounting.Application.Reports.Models
{
    public class ReportMetadata
    {
        public string ReportName { get; set; } = string.Empty;
        public string CompanyName { get; set; } = "ERP System";
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public string? GeneratedBy { get; set; }
        public string? CurrencyCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? AsOfDate { get; set; }
        public Dictionary<string, string> AppliedFilters { get; set; } = new();
    }
}
