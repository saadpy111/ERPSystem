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
    /// 
    /// NEW PRESENTATION FIELDS:
    /// - FontFamily for global website font
    /// - Text styling per hero element (FontSize, FontWeight, Color, Alignment, Spacing)
    /// - Image styling for hero background (BorderRadius, OverlayColor, OverlayOpacity)
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
        
        // ── Business data (always required) ─────────────────────────────────
        public string SiteName { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string BusinessType { get; set; } = string.Empty;
        public string about_the_site { get; set; } = string.Empty;
        public string location { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        
        // ── Uploaded Images ──────────────────────────────────────────────────
        public IFormFile? Logo { get; set; }
        public IFormFile? HeroBackgroundImage { get; set; }
        
        // ── Colors (required ONLY for Custom mode, ignored for Theme mode) ───
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? BackgroundColor { get; set; }
        public string? TextColor { get; set; }
        public string? FontFamily { get; set; }
        
        // ── Hero Title ───────────────────────────────────────────────────────
        public string? HeroTitle { get; set; }
        public int? HeroTitleFontSize { get; set; }
        public WebsiteFontWeight? HeroTitleFontWeight { get; set; }
        public string? HeroTitleColor { get; set; }
        public WebsiteTextAlign? HeroTitleAlignment { get; set; }
        public int? HeroTitleHorizontalSpacing { get; set; }
        public int? HeroTitleVerticalSpacing { get; set; }

        // ── Hero Subtitle ────────────────────────────────────────────────────
        public string? HeroSubtitle { get; set; }
        public int? HeroSubtitleFontSize { get; set; }
        public WebsiteFontWeight? HeroSubtitleFontWeight { get; set; }
        public string? HeroSubtitleColor { get; set; }
        public WebsiteTextAlign? HeroSubtitleAlignment { get; set; }
        public int? HeroSubtitleHorizontalSpacing { get; set; }
        public int? HeroSubtitleVerticalSpacing { get; set; }

        // ── Hero Button Text ─────────────────────────────────────────────────
        public string? HeroButtonText { get; set; }
        public int? HeroButtonTextFontSize { get; set; }
        public WebsiteFontWeight? HeroButtonTextFontWeight { get; set; }
        public string? HeroButtonTextColor { get; set; }
        public WebsiteTextAlign? HeroButtonTextAlignment { get; set; }
        public int? HeroButtonTextHorizontalSpacing { get; set; }
        public int? HeroButtonTextVerticalSpacing { get; set; }

        // ── Hero Background Image Style ──────────────────────────────────────
        public int? HeroBackgroundBorderRadius { get; set; }
        public string? HeroBackgroundOverlayColor { get; set; }
        public int? HeroBackgroundOverlayOpacity { get; set; }
        
        public List<WebsiteSection>? Sections { get; set; }
    }
}
