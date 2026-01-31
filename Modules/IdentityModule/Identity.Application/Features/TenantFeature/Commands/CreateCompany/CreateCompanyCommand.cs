using MediatR;
using SharedKernel.Enums;
using SharedKernel.Website;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Identity.Application.Features.TenantFeature.Commands.CreateCompany
{
    /// <summary>
    /// Command to create a new company (tenant).
    /// 
    /// CROSS-MODULE RESPONSIBILITIES:
    /// - IdentityModule: Tenant, User, Roles, passes website data to WebsiteModule
    /// - SubscriptionModule: Subscription creation
    /// - WebsiteModule: Theme loading, validation, SiteConfig creation
    /// 
    /// WEBSITE CONFIGURATION RULES:
    /// - If ThemeCode is provided: Presentation data (Colors, Hero, Sections) is IGNORED
    /// - If ThemeCode is null: Presentation data is REQUIRED
    /// </summary>
    public class CreateCompanyCommand : IRequest<CreateCompanyResponse>
    {
        // ===== IDENTITY DATA =====
        public string UserId { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyCode { get; set; } = string.Empty;
        
        // ===== SUBSCRIPTION DATA =====
        public string PlanCode { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public BillingInterval Interval { get; set; } = BillingInterval.Monthly;
        
        // ===== WEBSITE DATA =====
        
        /// <summary>
        /// Optional theme code.
        /// If provided: Theme mode - presentation from theme, user presentation IGNORED.
        /// If null: Custom mode - presentation from user (REQUIRED).
        /// </summary>
        public string? ThemeCode { get; set; }
        
        // Business data (always required)
        public string SiteName { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string BusinessType { get; set; } = string.Empty;
        
        // Uploaded Images
        public IFormFile? Logo { get; set; }
        public IFormFile? HeroBackgroundImage { get; set; }
        
        // Presentation data (required ONLY for Custom mode, ignored for Theme mode)
        // Flattened to keep request flat for multipart/form-data support
        
        // Colors
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? BackgroundColor { get; set; }
        public string? TextColor { get; set; }
        
        // Hero Text
        public string? HeroTitle { get; set; }
        public string? HeroSubtitle { get; set; }
        public string? HeroButtonText { get; set; }
        
        public List<WebsiteSection>? Sections { get; set; }
    }
}
