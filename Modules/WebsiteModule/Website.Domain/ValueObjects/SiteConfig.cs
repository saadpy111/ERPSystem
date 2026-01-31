using System.Collections.Generic;

namespace Website.Domain.ValueObjects
{
    /// <summary>
    /// Complete tenant website configuration stored as JSON.
    /// Contains both business data (always user-controlled) and presentation data.
    /// </summary>
    public class SiteConfig
    {
        // ===== BUSINESS DATA (ALWAYS user-controlled, NEVER overridden by theme) =====
        public string SiteName { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string BusinessType { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        
        // ===== PRESENTATION DATA (from user OR copied from theme) =====
        public ThemeColors Colors { get; set; } = new();
        public HeroSection Hero { get; set; } = new();
        public List<SectionItem> Sections { get; set; } = new();
    }

}
