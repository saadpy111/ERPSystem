using MediatR;
using Microsoft.AspNetCore.Http;
using SharedKernel.Website;

namespace Website.Application.Features.WebsiteInitialization.Commands.InitializeWebsite
{
    public class InitializeWebsiteCommand : IRequest<InitializeWebsiteResponse>
    {
        public string TenantId { get; set; } = string.Empty;

        public string? ThemeCode { get; set; }

        public string SiteName { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string BusinessType { get; set; } = string.Empty;
        public string about_the_site { get; set; } = string.Empty;
        public string location { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;

        public IFormFile? Logo { get; set; }
        public IFormFile? HeroBackgroundImage { get; set; }
        public IFormFile? ContactUsImg { get; set; }
        public IFormFile? ClientOImg { get; set; }

        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? BackgroundColor { get; set; }
        public string? TextColor { get; set; }
        public string? FontFamily { get; set; }

        public string? HeroTitle { get; set; }
        public int? HeroTitleFontSize { get; set; }
        public WebsiteFontWeight? HeroTitleFontWeight { get; set; }
        public string? HeroTitleColor { get; set; }
        public WebsiteTextAlign? HeroTitleAlignment { get; set; }
        public int? HeroTitleHorizontalSpacing { get; set; }
        public int? HeroTitleVerticalSpacing { get; set; }

        public string? HeroSubtitle { get; set; }
        public int? HeroSubtitleFontSize { get; set; }
        public WebsiteFontWeight? HeroSubtitleFontWeight { get; set; }
        public string? HeroSubtitleColor { get; set; }
        public WebsiteTextAlign? HeroSubtitleAlignment { get; set; }
        public int? HeroSubtitleHorizontalSpacing { get; set; }
        public int? HeroSubtitleVerticalSpacing { get; set; }

        public string? HeroButtonText { get; set; }
        public int? HeroButtonTextFontSize { get; set; }
        public WebsiteFontWeight? HeroButtonTextFontWeight { get; set; }
        public string? HeroButtonTextColor { get; set; }
        public WebsiteTextAlign? HeroButtonTextAlignment { get; set; }
        public int? HeroButtonTextHorizontalSpacing { get; set; }
        public int? HeroButtonTextVerticalSpacing { get; set; }

        public int? HeroBackgroundBorderRadius { get; set; }
        public string? HeroBackgroundOverlayColor { get; set; }
        public int? HeroBackgroundOverlayOpacity { get; set; }

        public List<WebsiteSection>? Sections { get; set; }
    }
}
