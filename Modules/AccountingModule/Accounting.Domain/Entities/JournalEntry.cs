using Accounting.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Accounting.Domain.Entities
{
    public class JournalEntry
    {
        public int Id { get; set; }
        public string JournalNumber { get; set; } = null!;
        public DateTime Date { get; set; }
        public string? Reference { get; set; }
        public int CurrencyId { get; set; }
        public SourceType SourceType { get; set; }
        public string? Description { get; set; }
        public JournalStatus Status { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public int FiscalPeriodId { get; set; }
        public int TenantId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PostedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Currency Currency { get; set; } = null!;
        public virtual FiscalPeriod FiscalPeriod { get; set; } = null!;
        public virtual ICollection<JournalEntryLine> Lines { get; set; } = new List<JournalEntryLine>();
        public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
    }
}
