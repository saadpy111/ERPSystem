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
        public int? CostCenterId { get; set; }

        // Multi-currency: locked at voucher creation time.
        // For Journal Vouchers entered manually, user provides ForeignAmount + ExchangeRate.
        // BaseAmount = ForeignAmount × ExchangeRate. Debit/Credit hold base-currency values.
        public decimal? ForeignAmount { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? BaseAmount { get; set; }

        public virtual Voucher Voucher { get; set; } = null!;
        public virtual Account Account { get; set; } = null!;
        public virtual Currency Currency { get; set; } = null!;
        public virtual CostCenter? CostCenter { get; set; }
    }
}
