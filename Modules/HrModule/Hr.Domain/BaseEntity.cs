using Hr.Domain.Entities;
using System;

namespace Hr.Domain
{
    public abstract class BaseEntity : ITenantEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string TenantId { get; set; } = null!;
    }
}
