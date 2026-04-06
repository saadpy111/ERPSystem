using Accounting.Domain.Common;
using System.Collections.Generic;

namespace Accounting.Domain.Entities
{
    public class CashAccount : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int AccountId { get; set; } // Map to GL Account

        public virtual Account Account { get; set; } = null!;
        public virtual ICollection<CashTransaction> Transactions { get; set; } = new List<CashTransaction>();
    }
}
