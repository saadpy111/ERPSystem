using System.Text.Json.Serialization;
using Website.Domain.Enums;
using Website.Domain.ValueObjects;

namespace Website.Application.DTOs
{
    /// <summary>
    /// DTO for TenantWebsite entity.
    /// </summary>
    public class TenantWebsiteDto
    {
        public Guid Id { get; set; }
        public string TenantId { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]

        public WebsiteMode Mode { get; set; }
        public Guid? ThemeId { get; set; }
        public SiteConfig Config { get; set; } = new();
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
