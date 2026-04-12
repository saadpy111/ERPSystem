namespace Accounting.Application.Dashboard.DTOs
{
    public class ProfitByCostCenterDto
    {
        public int? CostCenterId { get; init; }
        public string? CostCenterName { get; init; }
        public decimal Revenue { get; init; }
        public decimal Expenses { get; init; }
        public decimal Profit { get; init; }
    }
}
