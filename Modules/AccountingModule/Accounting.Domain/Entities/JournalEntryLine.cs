using Accounting.Domain.Common;

namespace Accounting.Domain.Entities
{
    public class JournalEntryLine : BaseEntity
    {
        public int JournalEntryId { get; set; }
        public int AccountId { get; set; }
        public string? Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public int? PartnerId { get; set; }
        public int? CostCenterId { get; set; }
        public int CurrencyId { get; set; }
        public decimal? ForeignAmount { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal BaseAmount { get; set; }
        public virtual JournalEntry JournalEntry { get; set; } = null!;
        public virtual Account Account { get; set; } = null!;
        public virtual Partner? Partner { get; set; }
        public virtual CostCenter? CostCenter { get; set; }
        public virtual Currency Currency { get; set; } = null!;
    }
}
