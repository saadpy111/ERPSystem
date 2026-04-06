using Accounting.Domain.Common;
using System.Collections.Generic;

namespace Accounting.Domain.Entities
{
    public class Tax : BaseEntity
    {
        public string Name { get; set; } = null!;
        public decimal Rate { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Partner> Partners { get; set; } = new List<Partner>();
    }
}
