using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    /// <summary>
    /// Base entity for all Inventory module entities.
    /// Implements ITenantEntity for multi-tenant data isolation.
    /// </summary>
    public class BaseEntity : ITenantEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Tenant identifier for multi-tenant data isolation.
        /// Required and automatically filtered by global query filters.
        /// </summary>
        public string TenantId { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
