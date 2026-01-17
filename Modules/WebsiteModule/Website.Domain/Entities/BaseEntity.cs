namespace Website.Domain.Entities
{
    /// <summary>
    /// Base entity for all Website module e-commerce entities.
    /// Implements ITenantEntity for multi-tenant data isolation.
    /// </summary>
    public abstract class BaseEntity : ITenantEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Tenant identifier for multi-tenant data isolation.
        /// </summary>
        public string TenantId { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
