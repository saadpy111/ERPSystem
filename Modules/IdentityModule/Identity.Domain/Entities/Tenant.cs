using System;
using System.Collections.Generic;

namespace Identity.Domain.Entities
{
    /// <summary>
    /// Tenant entity for multi-tenant isolation.
    /// Contains ONLY auth/isolation concerns - no website/theme data.
    /// </summary>
    public class Tenant
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public virtual ICollection<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();
    }
}
