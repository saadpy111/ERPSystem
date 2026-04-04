using System;
using System.Collections.Generic;

namespace Accounting.Domain.Entities
{
    public class FiscalYear
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsClosed { get; set; }
        public int TenantId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<FiscalPeriod> FiscalPeriods { get; set; } = new List<FiscalPeriod>();
    }
}
