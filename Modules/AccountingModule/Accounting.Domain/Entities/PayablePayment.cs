using System;
using Accounting.Domain.Common;

namespace Accounting.Domain.Entities
{
    public class PayablePayment : BaseEntity
    {
        public int PayableId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int CashAccountId { get; set; }
        public string? Description { get; set; }
        public int CurrencyId { get; set; }
        public decimal ExchangeRate { get; set; } = 1m;
        public decimal ForeignAmount { get; set; }
        public decimal BaseAmount { get; set; }

        public virtual Payable Payable { get; set; } = null!;
        public virtual CashAccount CashAccount { get; set; } = null!;
    }
}
