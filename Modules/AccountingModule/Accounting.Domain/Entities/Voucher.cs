using Accounting.Domain.Enums;
using System;

namespace Accounting.Domain.Entities
{
    public class Voucher
    {
        public int Id { get; set; }
        public string VoucherNumber { get; set; } = null!;
        public VoucherType VoucherType { get; set; }
        public DateTime Date { get; set; }
        public int PartnerId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public int CurrencyId { get; set; }
        public decimal Amount { get; set; }
        public JournalStatus Status { get; set; }
        public int JournalEntryId { get; set; }
        public int TenantId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Partner Partner { get; set; } = null!;
        public virtual Currency Currency { get; set; } = null!;
        public virtual JournalEntry JournalEntry { get; set; } = null!;
    }
}
