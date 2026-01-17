using System.Collections.Generic;

namespace Website.Domain.ValueObjects
{
    /// <summary>
    /// Theme configuration stored as JSON in Theme entity.
    /// Contains presentation data only.
    /// </summary>
    public class ThemeConfig
    {
        public ThemeColors Colors { get; set; } = new();
        public HeroSection Hero { get; set; } = new();
        public List<SectionItem> Sections { get; set; } = new();
    }
}
