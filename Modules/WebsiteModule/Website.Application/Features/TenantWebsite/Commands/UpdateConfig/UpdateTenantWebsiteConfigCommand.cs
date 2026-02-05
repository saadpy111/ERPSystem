using MediatR;
using Website.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

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
        public string? about_the_site { get; set; }
        public string? location { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        
        // Uploaded Images
        public IFormFile? Logo { get; set; }
        public IFormFile? HeroBackgroundImage { get; set; }
        
        // Presentation data (if provided, mode becomes Custom)
        // Flattened for multipart/form-data support
        
        // Colors
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? BackgroundColor { get; set; }
        public string? TextColor { get; set; }
        
        // Hero Text
        public string? HeroTitle { get; set; }
        public string? HeroSubtitle { get; set; }
        public string? HeroButtonText { get; set; }
        
        public List<SectionItem>? Sections { get; set; }
        
        public bool? IsPublished { get; set; }
    }

    public class UpdateTenantWebsiteConfigResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
