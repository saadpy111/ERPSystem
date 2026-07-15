using Website.Domain.Enums;

namespace Website.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public decimal CurrentBalance { get; set; }

        public ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();
        public ICollection<WithdrawalRequest> WithdrawalRequests { get; set; } = new List<WithdrawalRequest>();
    }
}
