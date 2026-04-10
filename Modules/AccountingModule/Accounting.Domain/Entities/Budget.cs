using Accounting.Domain.Common;
using Accounting.Domain.Enums;
using System.Collections.Generic;

namespace Accounting.Domain.Entities
{
    public class Budget : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int FiscalYearId { get; set; }
        public BudgetStatus Status { get; set; } = BudgetStatus.Draft;
        public bool EnforceBudgetControl { get; set; }

        public virtual FiscalYear FiscalYear { get; set; } = null!;
        public virtual ICollection<BudgetLine> Lines { get; set; } = new List<BudgetLine>();
    }
}
