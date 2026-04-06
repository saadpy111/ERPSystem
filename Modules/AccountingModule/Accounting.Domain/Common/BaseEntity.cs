using Accounting.Domain.Common.Interfaces;
using System;

namespace Accounting.Domain.Common
{
    public abstract class BaseEntity : IMultiTenant, IAuditable, ISoftDelete
    {
        public int Id { get; set; }
        public Guid TenantId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
