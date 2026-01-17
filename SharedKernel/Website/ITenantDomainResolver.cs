namespace SharedKernel.Website
{
    /// <summary>
    /// Interface for resolving tenant by domain name.
    /// Implemented by WebsiteModule, consumed by IdentityModule.
    /// Used for client authentication (storefront customers).
    /// </summary>
    public interface ITenantDomainResolver
    {
        /// <summary>
        /// Get tenant information by domain name.
        /// </summary>
        /// <param name="domain">The storefront domain (e.g., "mystore.com")</param>
        /// <returns>Tenant info if found, null otherwise</returns>
        Task<TenantDomainResult?> GetTenantByDomainAsync(string domain);
    }

    /// <summary>
    /// Result of domain lookup.
    /// </summary>
    public class TenantDomainResult
    {
        public string TenantId { get; set; } = string.Empty;
        public string TenantName { get; set; } = string.Empty;
        public string StoreName { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
    }
}
