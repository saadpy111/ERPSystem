using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Domain.Entities
{
    public interface ITenantEntity
    {
        public string TenantId { get; set; }
    }
}
