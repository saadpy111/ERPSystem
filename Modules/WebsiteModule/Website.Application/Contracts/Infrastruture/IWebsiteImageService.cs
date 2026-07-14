using Microsoft.AspNetCore.Http;

namespace Website.Application.Contracts.Infrastruture
{
    /// <summary>
    /// Contract for processing and storing website-related images.
    /// </summary>
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
