using Accounting.Domain.Common;
using System.Collections.Generic;

namespace Accounting.Domain.Entities
{
    public class FiscalPeriod : BaseEntity
    {
        public int FiscalYearId { get; set; }
        public string PeriodName { get; set; } = null!;
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool IsClosed { get; set; }

        public virtual FiscalYear FiscalYear { get; set; } = null!;
        public virtual ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();
    }
}
