using Accounting.Domain.Common;
using System.Collections.Generic;

namespace Accounting.Domain.Entities
{
    public class FiscalYear : BaseEntity
    {
        public string Name { get; set; } = null!;
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool IsClosed { get; set; }

        public virtual ICollection<FiscalPeriod> FiscalPeriods { get; set; } = new List<FiscalPeriod>();
    }
}
