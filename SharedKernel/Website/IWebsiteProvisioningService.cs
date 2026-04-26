using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SharedKernel.Website
{
    /// <summary>
    /// Request model for initializing a tenant's website configuration.
    ///
    /// RULES:
    /// - If ThemeCode is provided → Presentation data (Colors, Hero, Sections) is IGNORED
    /// - If ThemeCode is null → Presentation data is USED
    /// </summary>
    public class WebsiteInitializationRequest
    {
        public string? ThemeCode { get; set; }

    // ===== BUSINESS DATA =====
    public string SiteName { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string BusinessType { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string about_the_site { get; set; } = string.Empty;
        public string location { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;

        // ===== PRESENTATION DATA (Custom mode only) =====
        public WebsiteColors? Colors { get; set; }
        public WebsiteHero? Hero { get; set; }
        public List<WebsiteSection>? Sections { get; set; }
        public WebsiteContactUsImages? ContactUsImages { get; set; }
    }

    // ================= CONTACT IMAGES =================

    public class WebsiteContactUsImages
    {
        public WebsiteImageContent? ContactUsImg { get; set; }
        public WebsiteImageContent? ClientOImg { get; set; }
    }

    // ================= COLORS =================

    public class WebsiteColors
    {
        public string Primary { get; set; } = string.Empty;
        public string Secondary { get; set; } = string.Empty;
        public string Background { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string FontFamily { get; set; } = "Neo Sans Arabic";
    }

    // ================= ENUMS =================

    public enum WebsiteFontWeight
    {
        Light = 0,
        Normal = 1,
        Bold = 2
    }

    public enum WebsiteTextAlign
    {
        Left = 0,
        Center = 1,
        Right = 2
    }

    // ================= TEXT =================

    public class WebsiteTextStyle
    {
        public int FontSize { get; set; } = 16;
        public WebsiteFontWeight FontWeight { get; set; } = WebsiteFontWeight.Normal;
        public string Color { get; set; } = "#000000";
        public WebsiteTextAlign Alignment { get; set; } = WebsiteTextAlign.Left;
        public int HorizontalSpacing { get; set; } = 0;
        public int VerticalSpacing { get; set; } = 0;
        public int MarginTop { get; set; } = 0;
    }

    public class WebsiteTextContent
    {
        public string Text { get; set; } = string.Empty;
        public WebsiteTextStyle Style { get; set; } = new();
    }

    // ================= IMAGE =================

    public class WebsiteImageStyle
    {
        public int BorderRadius { get; set; } = 6;
        public string OverlayColor { get; set; } = "#FFFFFF";
        public int OverlayOpacity { get; set; } = 40;
    }

    public class WebsiteImageContent
    {
        public string Url { get; set; } = string.Empty;
        public WebsiteImageStyle Style { get; set; } = new();
    }

    // ================= HERO =================

    public class WebsiteHero
    {
        public WebsiteTextContent Title { get; set; } = new();
        public WebsiteTextContent Subtitle { get; set; } = new();
        public WebsiteTextContent ButtonText { get; set; } = new();
        public WebsiteImageContent BackgroundImage { get; set; } = new();
    }

    // ================= SECTION (🔥 أهم تعديل) =================

    public class WebsiteSection
    {
        // Identity
        public string? Id { get; set; }
        public bool? Enabled { get; set; }
        public int? Order { get; set; }

        // 🔥 Full Control زي Hero
        public WebsiteTextContent? Title { get; set; }
        public WebsiteTextContent? Subtitle { get; set; }
        public WebsiteTextContent? ButtonText { get; set; }
        public WebsiteImageContent? BackgroundImage { get; set; }
    }

    // ================= RESULT =================

    public class WebsiteProvisioningResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }

    // ================= SERVICE =================

    public interface IWebsiteProvisioningService
    {
        Task<WebsiteProvisioningResult> InitializeTenantWebsiteAsync(
            string tenantId,
            WebsiteInitializationRequest request);
    }

    // ================= IMAGE SERVICE =================

    public interface IWebsiteImageService
    {
        Task<string> ProcessWebsiteLogoAsync(string tenantId, IFormFile logoFile);
        Task<string> ProcessWebsiteHeroImageAsync(string tenantId, IFormFile heroFile);
        Task<string> ProcessThemePreviewImageAsync(string themeCode, IFormFile previewFile);
        Task<string> ProcessThemeHeroImageAsync(string themeCode, IFormFile heroFile);
        Task<string> ProcessWebsiteContactUsImgAsync(string tenantId, IFormFile file);
        Task<string> ProcessWebsiteClientOImgAsync(string tenantId, IFormFile file);
        Task<string> ProcessThemeContactUsImgAsync(string themeCode, IFormFile file);
        Task<string> ProcessThemeClientOImgAsync(string themeCode, IFormFile file);
        Task<string> ProcessWebsiteSectionImageAsync(string tenantId, string sectionId, IFormFile file);
    }

}
