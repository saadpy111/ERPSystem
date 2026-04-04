using System;
using System.Collections.Generic;

namespace Accounting.Domain.Entities
{
    public class CostCenter
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
        public int TenantId { get; set; }

        public virtual CostCenter? Parent { get; set; }
        public virtual ICollection<CostCenter> Children { get; set; } = new List<CostCenter>();
        public virtual ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
    }
}
