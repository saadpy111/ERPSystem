using Accounting.Domain.Common;
using System;

namespace Accounting.Domain.Entities
{
    public class BudgetLine : BaseEntity
    {
        public int BudgetId { get; set; }
        public int AccountId { get; set; }
        public int? CostCenterId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PlannedAmount { get; set; }

        public virtual Budget Budget { get; set; } = null!;
        public virtual Account Account { get; set; } = null!;
        public virtual CostCenter? CostCenter { get; set; }
    }
}
