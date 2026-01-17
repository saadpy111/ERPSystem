namespace Website.Domain.Entities
{
    /// <summary>
    /// Interface for entities that are scoped to a specific tenant.
    /// </summary>
    public interface ITenantEntity
    {
        string TenantId { get; set; }
    }
}
