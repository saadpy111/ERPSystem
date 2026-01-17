using System.Collections.Generic;

namespace SharedKernel.Website
{
    /// <summary>
    /// Request model for initializing a tenant's website configuration.
    /// Used for cross-module communication between IdentityModule and WebsiteModule.
    /// 
    /// STRICT RULES:
    /// - If ThemeCode is provided → Presentation data (Colors, Hero, Sections) is IGNORED
    /// - If ThemeCode is null → Presentation data is REQUIRED (validation error if missing)
    /// </summary>
    public class WebsiteInitializationRequest
    {
        /// <summary>
        /// Optional theme code to apply.
        /// If provided: Theme mode - presentation comes from theme.
        /// If null: Custom mode - presentation comes from user (REQUIRED).
        /// </summary>
        public string? ThemeCode { get; set; }
        
        // ===== BUSINESS DATA (always from user, always required) =====
        public string SiteName { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string BusinessType { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        
        // ===== PRESENTATION DATA (for Custom mode ONLY) =====
        // These are IGNORED when ThemeCode is provided.
        // These are REQUIRED when ThemeCode is null.
        
        public WebsiteColors? Colors { get; set; }
        public WebsiteHero? Hero { get; set; }
        public List<WebsiteSection>? Sections { get; set; }
    }

    /// <summary>
    /// Color configuration for website.
    /// REQUIRED for Custom mode. IGNORED for Theme mode.
    /// </summary>
    public class WebsiteColors
    {
        public string Primary { get; set; } = string.Empty;
        public string Secondary { get; set; } = string.Empty;
        public string Background { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }

    /// <summary>
    /// Hero section configuration for website.
    /// REQUIRED for Custom mode. IGNORED for Theme mode.
    /// </summary>
    public class WebsiteHero
    {
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string ButtonText { get; set; } = string.Empty;
        public string BackgroundImage { get; set; } = string.Empty;
    }

    /// <summary>
    /// Section configuration for website.
    /// REQUIRED for Custom mode. IGNORED for Theme mode.
    /// </summary>
    public class WebsiteSection
    {
        public string Id { get; set; } = string.Empty;
        public bool Enabled { get; set; } = true;
        public int Order { get; set; }
    }

    /// <summary>
    /// Result of website provisioning operation.
    /// </summary>
    public class WebsiteProvisioningResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }

    /// <summary>
    /// Cross-module interface for website provisioning.
    /// Implemented by WebsiteModule, consumed by IdentityModule.
    /// 
    /// BUSINESS RULES (NON-NEGOTIABLE):
    /// 
    /// THEME MODE (ThemeCode provided):
    /// - Business data: ALWAYS from user
    /// - Presentation data: ALWAYS from theme (user input IGNORED)
    /// 
    /// CUSTOM MODE (ThemeCode null):
    /// - Business data: ALWAYS from user
    /// - Presentation data: ALWAYS from user (REQUIRED, no defaults)
    /// </summary>
    public interface IWebsiteProvisioningService
    {
        Task<WebsiteProvisioningResult> InitializeTenantWebsiteAsync(
            string tenantId,
            WebsiteInitializationRequest request);
    }
}
