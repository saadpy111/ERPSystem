using System;
using Website.Domain.Enums;
using Website.Domain.ValueObjects;

namespace Website.Domain.Entities
{
    /// <summary>
    /// TenantWebsite entity - Per-tenant website configuration.
    /// Contains snapshot of configuration (not live-linked to theme).
    /// </summary>
    public class TenantWebsite
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        /// <summary>
        /// Reference to Identity.Tenant (string ID, no navigation property)
        /// </summary>
        public string TenantId { get; set; } = string.Empty;
        
        /// <summary>
        /// How the website is configured (Custom or Theme)
        /// </summary>
        public WebsiteMode Mode { get; set; } = WebsiteMode.Custom;
        
        /// <summary>
        /// If Mode=Theme, which theme was applied (for reference only)
        /// </summary>
        public Guid? ThemeId { get; set; }
        
        /// <summary>
        /// Complete website configuration snapshot (JSON)
        /// Contains both business data and presentation data
        /// </summary>
        public SiteConfig Config { get; set; } = new();
        
        /// <summary>
        /// Whether the website is published/live
        /// </summary>
        public bool IsPublished { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
