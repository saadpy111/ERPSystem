using Accounting.Domain.Common;
using Accounting.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Accounting.Domain.Entities
{
    public class JournalEntry : BaseEntity
    {
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
        public DateTime? PostedAt { get; set; }

        // Reversal Tracking
        public bool IsReversed { get; set; }
        public int? ReversedEntryId { get; set; }
        public DateTime? ReversedAt { get; set; }
        public string? ReversedBy { get; set; }
        public string? ReversalReason { get; set; }

        public virtual Currency Currency { get; set; } = null!;
        public virtual FiscalPeriod FiscalPeriod { get; set; } = null!;
        public virtual JournalEntry? ReversedEntry { get; set; }
        public virtual ICollection<JournalEntry> Reversals { get; set; } = new List<JournalEntry>();
        public virtual ICollection<JournalEntryLine> Lines { get; set; } = new List<JournalEntryLine>();
        public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
    }
}
