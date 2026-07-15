using Website.Domain.Enums;

namespace Website.Domain.Entities
{
    public class WithdrawalRequest : BaseEntity
    {
        public Guid WalletId { get; set; }
        public Wallet Wallet { get; set; } = null!;
        public decimal Amount { get; set; }
        public WithdrawalRequestStatus Status { get; set; } = WithdrawalRequestStatus.Pending;
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReviewedAt { get; set; }
        public string? ReviewedBy { get; set; }
        public string? Notes { get; set; }
    }
}
