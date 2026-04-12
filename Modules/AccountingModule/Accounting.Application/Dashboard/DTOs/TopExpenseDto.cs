namespace Accounting.Application.Dashboard.DTOs
{
    public class TopExpenseDto
    {
        public int AccountId { get; init; }
        public string AccountCode { get; init; } = null!;
        public string AccountName { get; init; } = null!;
        public decimal TotalExpense { get; init; }
    }
}
