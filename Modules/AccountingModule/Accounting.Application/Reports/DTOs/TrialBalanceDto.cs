namespace Accounting.Application.Reports.DTOs
{
    public class TrialBalanceDto
    {
        public int AccountId { get; set; }
        public string AccountCode { get; set; } = null!;
        public string AccountName { get; set; } = null!;
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public int? CostCenterId { get; set; }
        public string? CostCenterName { get; set; }
    }
}
