using MediatR;
using Website.Domain.Enums;
using Website.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Website.Application.Features.Themes.Commands.UpdateTheme
{
    public class UpdateThemeCommand : IRequest<UpdateThemeResponse>
    {
        public Guid ThemeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        
        // ── Uploaded Images ──────────────────────────────────────────────────
        public IFormFile? PreviewImageFile { get; set; }
        public IFormFile? HeroBackgroundImageFile { get; set; }
        
        // ── Colors ──────────────────────────────────────────────────────────
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? BackgroundColor { get; set; }
        public string? TextColor { get; set; }
        public string? FontFamily { get; set; }
        
        // ── Hero Title ───────────────────────────────────────────────────────
        public string? HeroTitle { get; set; }
        public int? HeroTitleFontSize { get; set; }
        public FontWeight? HeroTitleFontWeight { get; set; }
        public string? HeroTitleColor { get; set; }
        public TextAlign? HeroTitleAlignment { get; set; }
        public int? HeroTitleHorizontalSpacing { get; set; }
        public int? HeroTitleVerticalSpacing { get; set; }

        // ── Hero Subtitle ────────────────────────────────────────────────────
        public string? HeroSubtitle { get; set; }
        public int? HeroSubtitleFontSize { get; set; }
        public FontWeight? HeroSubtitleFontWeight { get; set; }
        public string? HeroSubtitleColor { get; set; }
        public TextAlign? HeroSubtitleAlignment { get; set; }
        public int? HeroSubtitleHorizontalSpacing { get; set; }
        public int? HeroSubtitleVerticalSpacing { get; set; }

        // ── Hero Button Text ─────────────────────────────────────────────────
        public string? HeroButtonText { get; set; }
        public int? HeroButtonTextFontSize { get; set; }
        public FontWeight? HeroButtonTextFontWeight { get; set; }
        public string? HeroButtonTextColor { get; set; }
        public TextAlign? HeroButtonTextAlignment { get; set; }
        public int? HeroButtonTextHorizontalSpacing { get; set; }
        public int? HeroButtonTextVerticalSpacing { get; set; }

        // ── Hero Background Image Style ──────────────────────────────────────
        public int? HeroBackgroundBorderRadius { get; set; }
        public string? HeroBackgroundOverlayColor { get; set; }
        public int? HeroBackgroundOverlayOpacity { get; set; }
        
        // ── Sections ─────────────────────────────────────────────────────────
        public List<SectionItem>? Sections { get; set; }
    }

    public class UpdateThemeResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
