using MediatR;
using Website.Domain.ValueObjects;

namespace Website.Application.Features.TenantWebsite.Commands.UpdateConfig
{
    /// <summary>
    /// Update tenant website configuration.
    /// Allows updating both business data and presentation data.
    /// If presentation data is provided, mode becomes Custom.
    /// </summary>
    public class UpdateTenantWebsiteConfigCommand : IRequest<UpdateTenantWebsiteConfigResponse>
    {
        public string TenantId { get; set; } = string.Empty;
        
        // Business data
        public string? SiteName { get; set; }
        public string? Domain { get; set; }
        public string? BusinessType { get; set; }
        public string? LogoUrl { get; set; }
        
        // Presentation data (if provided, mode becomes Custom)
        public ThemeColors? Colors { get; set; }
        public HeroSection? Hero { get; set; }
        public List<SectionItem>? Sections { get; set; }
        
        public bool? IsPublished { get; set; }
    }

    public class UpdateTenantWebsiteConfigResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
