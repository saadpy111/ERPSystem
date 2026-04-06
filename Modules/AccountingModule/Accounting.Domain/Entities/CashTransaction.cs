using Accounting.Domain.Common;
using Accounting.Domain.Enums;
using System;

namespace Accounting.Domain.Entities
{
    public class CashTransaction : BaseEntity
    {
        public int CashAccountId { get; set; }
        public CashTransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = null!;
        public string? Reference { get; set; }
        public int CurrencyId { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal BaseAmount { get; set; }
        
        public int? PartnerId { get; set; }
        public int OffsetAccountId { get; set; } 
        
        public virtual CashAccount CashAccount { get; set; } = null!;
        public virtual Account OffsetAccount { get; set; } = null!;
        public virtual Partner? Partner { get; set; }
        public virtual Currency Currency { get; set; } = null!;
    }
}
