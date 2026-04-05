using System;
using System.Collections.Generic;

namespace Accounting.Domain.Entities
{
    public class CashAccount
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int AccountId { get; set; } // Map to GL Account
        public int TenantId { get; set; }
        
        public virtual Account Account { get; set; } = null!;
        public virtual ICollection<CashTransaction> Transactions { get; set; } = new List<CashTransaction>();
    }
}
