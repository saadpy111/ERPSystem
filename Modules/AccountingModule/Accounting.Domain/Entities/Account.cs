using Accounting.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Accounting.Domain.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string? NameEn { get; set; }
        public int? ParentAccountId { get; set; }
        public AccountType AccountType { get; set; }
        public bool IsGroup { get; set; }
        public int CurrencyId { get; set; }
        public bool AllowReconciliation { get; set; }
        public bool IsActive { get; set; }
        public int TenantId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Account? ParentAccount { get; set; }
        public virtual ICollection<Account> ChildAccounts { get; set; } = new List<Account>();
        public virtual Currency Currency { get; set; } = null!;
        public virtual ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
    }
}
