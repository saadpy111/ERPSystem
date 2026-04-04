using System;
using System.Collections.Generic;

namespace Accounting.Domain.Entities
{
    public class Tax
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Rate { get; set; }
        public bool IsActive { get; set; }
        public int TenantId { get; set; }

        public virtual ICollection<Partner> Partners { get; set; } = new List<Partner>();
    }
}
