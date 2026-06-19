using Microsoft.AspNetCore.Http;
using Website.Domain.Enums;

namespace Website.Application.Features.TenantWebsite.Commands.UpdateConfig
{
    // ─────────────────────────────────────────────────────────────────────────
    //  Nullable patch DTOs — all fields are intentionally nullable so the
    //  system can distinguish "not provided" from "set to zero / false / empty".
    //  These are APPLICATION-layer types only; the domain ValueObjects are unchanged.
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Nullable style DTO for a text element.
    /// A null field means "do not change this property".
    /// A non-null field means "apply this value".
    ///
    /// Multipart example:
    ///   Sections[0].Title.Style.FontSize = 20
    ///   Sections[0].Title.Style.MarginTop = 0   ← explicitly setting to 0 is supported
    /// </summary>
    public class TextStyleDto
    {
        public int? FontSize { get; set; }
        public FontWeight? FontWeight { get; set; }
        public string? Color { get; set; }
        public TextAlign? Alignment { get; set; }
        public int? HorizontalSpacing { get; set; }
        public int? VerticalSpacing { get; set; }
        public int? MarginTop { get; set; }
        public string? BackgroundColor { get; set; }
    }

    /// <summary>
    /// Nullable text-content DTO.
    /// A null Text means "do not change the text".
    /// A null Style means "do not change any style property".
    /// </summary>
    public class TextContentDto
    {
        public string? Text { get; set; }
        public TextStyleDto? Style { get; set; }
    }

    /// <summary>
    /// Nullable style DTO for an image element.
    /// A null field means "do not change this property".
    /// </summary>
    public class ImageStyleDto
    {
        public int? BorderRadius { get; set; }
        public string? OverlayColor { get; set; }
        public int? OverlayOpacity { get; set; }
    }

    /// <summary>
    /// DTO for a single Section update request (PATCH semantics).
    ///
    /// Multipart/form-data example:
    ///   Sections[0].Id                         = about
    ///   Sections[0].Enabled                    = true
    ///   Sections[0].Order                      = 1
    ///   Sections[0].Title.Text                 = Hello
    ///   Sections[0].Title.Style.FontSize       = 20
    ///   Sections[0].Title.Style.MarginTop      = 0
    ///   Sections[0].BackgroundImageFile        = (binary file)
    ///   Sections[0].BackgroundImageStyle.BorderRadius = 8
    ///
    /// Rules:
    ///   - Id is the ONLY required field (used as the merge key)
    ///   - Every other field is optional — null = keep existing value
    ///   - BackgroundImageFile takes priority over BackgroundImageUrl
    ///   - If neither image field is provided the existing image URL is preserved
    /// </summary>
    public class SectionDto
    {
        // ── Merge key (REQUIRED) ──────────────────────────────────────────────
        public string Id { get; set; } = string.Empty;

        // ── Visibility / ordering ─────────────────────────────────────────────
        /// <summary>Null = keep existing value.</summary>
        public bool? Enabled { get; set; }

        /// <summary>Null = keep existing value.</summary>
        public int? Order { get; set; }

        // ── Text fields (all nullable — null object or null sub-fields = no change) ─
        public TextContentDto? Title { get; set; }
        public TextContentDto? Subtitle { get; set; }
        public TextContentDto? ButtonText { get; set; }

        // ── Background image ──────────────────────────────────────────────────
        /// <summary>
        /// File upload. If provided, the file is saved and the resulting URL
        /// replaces the existing one. Takes priority over BackgroundImageUrl.
        /// </summary>
        public IFormFile? BackgroundImageFile { get; set; }

        /// <summary>
        /// Direct URL. Applied only when BackgroundImageFile is null.
        /// If both are null the existing URL is preserved.
        /// </summary>
        public string? BackgroundImageUrl { get; set; }

        /// <summary>
        /// Optional image style override. Null = keep existing style.
        /// Individual null properties within the style = keep existing property.
        /// </summary>
        public ImageStyleDto? BackgroundImageStyle { get; set; }
    }
}
