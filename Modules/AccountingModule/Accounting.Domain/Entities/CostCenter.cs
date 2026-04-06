using Accounting.Domain.Common;
using System.Collections.Generic;

namespace Accounting.Domain.Entities
{
    public class CostCenter : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }

        public virtual CostCenter? Parent { get; set; }
        public virtual ICollection<CostCenter> Children { get; set; } = new List<CostCenter>();
        public virtual ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
    }
}
