using System;
using System.Collections.Generic;

namespace Accounting.Domain.Entities
{
    public class FiscalPeriod
    {
        public int Id { get; set; }
        public int FiscalYearId { get; set; }
        public string PeriodName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsClosed { get; set; }

        public virtual FiscalYear FiscalYear { get; set; } = null!;
        public virtual ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();
    }
}
