using MediatR;
using Website.Domain.Enums;
using Website.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Website.Application.Features.TenantWebsite.Commands.UpdateConfig
{
    /// <summary>
    /// Update tenant website configuration.
    /// Allows updating both business data and presentation data.
    /// If presentation data is provided, mode becomes Custom.
    /// 
    /// Hero text fields are flattened for multipart/form-data support.
    /// Text styling fields follow the pattern: Hero{Field}{StyleProp}.
    /// Image styling fields follow the pattern: HeroBackground{StyleProp}.
    /// </summary>
    public class UpdateTenantWebsiteConfigCommand : IRequest<UpdateTenantWebsiteConfigResponse>
    {
        public string TenantId { get; set; } = string.Empty;
        
        // ── Business data ────────────────────────────────────────────────────────
        public string? SiteName { get; set; }
        public string? Domain { get; set; }
        public string? BusinessType { get; set; }
        public string? about_the_site { get; set; }
        public string? location { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        
        // ── Uploaded Images ──────────────────────────────────────────────────────
        public IFormFile? Logo { get; set; }
        public IFormFile? HeroBackgroundImage { get; set; }

        // ── Contact Us Images (optional, partial updates supported) ──────────
        public IFormFile? ContactUsImg { get; set; }
        public IFormFile? ClientOImg { get; set; }

        // ── ContactUsImg Style ───────────────────────────────────────────────────
        public int? ContactUsImgBorderRadius { get; set; }
        public string? ContactUsImgOverlayColor { get; set; }
        public int? ContactUsImgOverlayOpacity { get; set; }

        // ── ClientOImg Style ─────────────────────────────────────────────────────
        public int? ClientOImgBorderRadius { get; set; }
        public string? ClientOImgOverlayColor { get; set; }
        public int? ClientOImgOverlayOpacity { get; set; }
        
        // ── Colors ──────────────────────────────────────────────────────────────
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? BackgroundColor { get; set; }
        public string? TextColor { get; set; }
        public string? FontFamily { get; set; }
        
        // ── Hero Title ───────────────────────────────────────────────────────────
        public string? HeroTitle { get; set; }
        public int? HeroTitleFontSize { get; set; }
        public FontWeight? HeroTitleFontWeight { get; set; }
        public string? HeroTitleColor { get; set; }
        public TextAlign? HeroTitleAlignment { get; set; }
        public int? HeroTitleHorizontalSpacing { get; set; }
        public int? HeroTitleVerticalSpacing { get; set; }

        // ── Hero Subtitle ────────────────────────────────────────────────────────
        public string? HeroSubtitle { get; set; }
        public int? HeroSubtitleFontSize { get; set; }
        public FontWeight? HeroSubtitleFontWeight { get; set; }
        public string? HeroSubtitleColor { get; set; }
        public TextAlign? HeroSubtitleAlignment { get; set; }
        public int? HeroSubtitleHorizontalSpacing { get; set; }
        public int? HeroSubtitleVerticalSpacing { get; set; }

        // ── Hero Button Text ─────────────────────────────────────────────────────
        public string? HeroButtonText { get; set; }
        public int? HeroButtonTextFontSize { get; set; }
        public FontWeight? HeroButtonTextFontWeight { get; set; }
        public string? HeroButtonTextColor { get; set; }
        public TextAlign? HeroButtonTextAlignment { get; set; }
        public int? HeroButtonTextHorizontalSpacing { get; set; }
        public int? HeroButtonTextVerticalSpacing { get; set; }

        // ── Hero Background Image Style ──────────────────────────────────────────
        public int? HeroBackgroundBorderRadius { get; set; }
        public string? HeroBackgroundOverlayColor { get; set; }
        public int? HeroBackgroundOverlayOpacity { get; set; }
        
        // ── Sections ─────────────────────────────────────────────────────────────
        public List<SectionItem>? Sections { get; set; }
        
        public bool? IsPublished { get; set; }
    }

    public class UpdateTenantWebsiteConfigResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
