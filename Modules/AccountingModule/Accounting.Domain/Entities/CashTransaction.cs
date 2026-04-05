using System;
using Accounting.Domain.Enums;

namespace Accounting.Domain.Entities
{
    public class CashTransaction
    {
        public int Id { get; set; }
        public int CashAccountId { get; set; }
        public CashTransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = null!;
        public string? Reference { get; set; }
        
        public int? PartnerId { get; set; }
        public int OffsetAccountId { get; set; } 
        
        public int TenantId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual CashAccount CashAccount { get; set; } = null!;
        public virtual Account OffsetAccount { get; set; } = null!;
        public virtual Partner? Partner { get; set; }
    }
}
