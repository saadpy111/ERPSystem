using Accounting.Domain.Common;
using System.Collections.Generic;

namespace Accounting.Domain.Entities
{
    public class CostCenter : BaseEntity
    {
        public string Code { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string? NameEn { get; set; }
        public int? ParentId { get; set; }
        public int? Level { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual CostCenter? Parent { get; set; }
        public virtual ICollection<CostCenter> Children { get; set; } = new List<CostCenter>();
        public virtual ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
    }
}
