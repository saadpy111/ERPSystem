using System.Collections.Generic;

namespace Accounting.Application.Reports.Models
{
    public class ReportResponse<TItem>
    {
        public List<TItem> Items { get; set; } = new();
        public Dictionary<string, decimal> Totals { get; set; } = new();
        public ReportMetadata Metadata { get; set; } = new();

        public bool Success { get; set; } = true;
        public string? Message { get; set; }

        public static ReportResponse<TItem> Failure(string message)
        {
            return new ReportResponse<TItem>
            {
                Success = false,
                Message = message
            };
        }
    }
}
