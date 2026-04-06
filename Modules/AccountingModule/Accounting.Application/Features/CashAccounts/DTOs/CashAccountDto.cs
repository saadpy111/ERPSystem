namespace Accounting.Application.Features.CashAccounts.DTOs
{
    public class CashAccountDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int AccountId { get; set; }
        public string AccountCode { get; set; } = null!;
        public string AccountName { get; set; } = null!;
    }
}
