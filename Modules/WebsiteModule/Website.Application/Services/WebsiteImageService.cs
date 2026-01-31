using SharedKernel.Website;
using Website.Application.Contracts.Infrastruture.FileService;
using Microsoft.AspNetCore.Http;

namespace Website.Application.Services
{
    public class WebsiteImageService : IWebsiteImageService
    {
        private readonly IFileService _fileService;

        public WebsiteImageService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<string> ProcessWebsiteLogoAsync(string tenantId, IFormFile logoFile)
        {
            if (logoFile == null || logoFile.Length == 0)
                throw new ArgumentException("Logo file is required", nameof(logoFile));

            string folderPath = $"websites/{tenantId}/logo";
            return await _fileService.SaveFileAsync(logoFile, folderPath);
        }

        public async Task<string> ProcessWebsiteHeroImageAsync(string tenantId, IFormFile heroFile)
        {
            if (heroFile == null || heroFile.Length == 0)
                throw new ArgumentException("Hero image file is required", nameof(heroFile));

            string folderPath = $"websites/{tenantId}/hero";
            return await _fileService.SaveFileAsync(heroFile, folderPath);
        }

        public async Task<string> ProcessThemePreviewImageAsync(string themeCode, IFormFile previewFile)
        {
            if (previewFile == null || previewFile.Length == 0)
                throw new ArgumentException("Preview image file is required", nameof(previewFile));

            string folderPath = $"themes/{themeCode}/preview";
            return await _fileService.SaveFileAsync(previewFile, folderPath);
        }

        public async Task<string> ProcessThemeHeroImageAsync(string themeCode, IFormFile heroFile)
        {
            if (heroFile == null || heroFile.Length == 0)
                throw new ArgumentException("Hero image file is required", nameof(heroFile));

            string folderPath = $"themes/{themeCode}/hero";
            return await _fileService.SaveFileAsync(heroFile, folderPath);
        }
    }
}
