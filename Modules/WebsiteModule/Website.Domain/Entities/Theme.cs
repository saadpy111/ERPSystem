using System;
using Website.Domain.ValueObjects;

namespace Website.Domain.Entities
{
    /// <summary>
    /// Theme entity - Global, Editable, Stored in Database.
    /// Defines reusable presentation templates.
    /// </summary>
    public class Theme
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        /// <summary>
        /// Unique code identifier (e.g., "tpl_furniture_01")
        /// </summary>
        public string Code { get; set; } = string.Empty;
        
        /// <summary>
        /// Display name (e.g., "أثاث عصري")
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Preview image URL
        /// </summary>
        public string PreviewImage { get; set; } = string.Empty;
        
        /// <summary>
        /// Whether this theme is available for selection
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// Theme configuration stored as JSON (Colors, Hero, Sections)
        /// </summary>
        public ThemeConfig Config { get; set; } = new();
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
