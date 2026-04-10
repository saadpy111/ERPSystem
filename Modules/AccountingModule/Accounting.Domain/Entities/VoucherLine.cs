using Accounting.Domain.Common;

namespace Accounting.Domain.Entities
{
    public class VoucherLine : BaseEntity
    {
        public int VoucherId { get; set; }
        public int AccountId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public int CurrencyId { get; set; }

        public virtual Voucher Voucher { get; set; } = null!;
        public virtual Account Account { get; set; } = null!;
        public virtual Currency Currency { get; set; } = null!;
    }
}
