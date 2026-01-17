namespace Inventory.Domain.Entities
{
    /// <summary>
    /// Interface for entities that are scoped to a specific tenant.
    /// All business data entities should implement this interface.
    /// </summary>
    public interface ITenantEntity
    {
        /// <summary>
        /// The unique identifier of the tenant that owns this entity.
        /// Required and indexed for query performance.
        /// </summary>
        string TenantId { get; set; }
    }
}
