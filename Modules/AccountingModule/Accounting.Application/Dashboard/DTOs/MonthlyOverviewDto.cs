namespace Accounting.Application.Dashboard.DTOs
{
    public class MonthlyOverviewDto
    {
        public int Year { get; init; }
        public int Month { get; init; }
        public string MonthName { get; init; } = null!;
        public decimal Revenue { get; init; }
        public decimal Expenses { get; init; }
        public decimal Profit { get; init; }
    }
}
