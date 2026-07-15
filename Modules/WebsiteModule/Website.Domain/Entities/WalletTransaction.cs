using Website.Domain.Enums;

namespace Website.Domain.Entities
{
    public class WalletTransaction : BaseEntity
    {
        public Guid WalletId { get; set; }
        public Wallet Wallet { get; set; } = null!;
        public WalletTransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
        public string? Reference { get; set; }
        public string? Description { get; set; }
    }
}
