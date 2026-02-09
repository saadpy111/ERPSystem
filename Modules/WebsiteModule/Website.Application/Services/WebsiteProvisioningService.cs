using SharedKernel.Website;
using Website.Application.Contracts.Persistence;
using Website.Domain.Entities;
using Website.Domain.Enums;
using Website.Domain.ValueObjects;

namespace Website.Application.Services
{
    /// <summary>
    /// Implementation of IWebsiteProvisioningService.
    /// 
    /// STRICT BUSINESS RULES (NON-NEGOTIABLE):
    /// ═══════════════════════════════════════════════════════════════
    /// CASE 1: THEME MODE
    /// ═══════════════════════════════════════════════════════════════

    /// - Business data: FROM USER
    /// - Colors & Hero: FROM THEME ONLY
    /// - Sections:
    ///   - From USER if provided
    ///   - Otherwise fallback to THEME
    /// - NO merging of Colors or Hero

    /// 
    /// ═══════════════════════════════════════════════════════════════
    /// CASE 2: CUSTOM MODE (ThemeCode is NULL)
    /// ═══════════════════════════════════════════════════════════════
    /// - Business data: FROM USER (required)
    /// - Presentation data: FROM USER (REQUIRED - validation error if missing)
    /// - NO backend defaults - we do NOT invent UI decisions
    /// 
    /// ═══════════════════════════════════════════════════════════════
    /// FORBIDDEN BEHAVIOR:
    /// ═══════════════════════════════════════════════════════════════
    /// - Setting default Colors, Hero, or Sections in backend code
    /// - Merging user input with Theme configuration
    /// - Allowing user input to override Theme presentation
    /// - Creating implicit UI decisions
    /// </summary>
    public class WebsiteProvisioningService : IWebsiteProvisioningService
    {
        private readonly IThemeRepository _themeRepository;
        private readonly ITenantWebsiteRepository _tenantWebsiteRepository;
        private readonly IWebsiteUnitOfWork _unitOfWork;

        public WebsiteProvisioningService(
            IThemeRepository themeRepository,
            ITenantWebsiteRepository tenantWebsiteRepository,
            IWebsiteUnitOfWork unitOfWork)
        {
            _themeRepository = themeRepository;
            _tenantWebsiteRepository = tenantWebsiteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WebsiteProvisioningResult> InitializeTenantWebsiteAsync(
            string tenantId,
            WebsiteInitializationRequest request)
        {
       

            // Check if tenant already has a website
            if (await _tenantWebsiteRepository.ExistsAsync(tenantId))
                return Fail("Tenant website already exists");

            TenantWebsite tenantWebsite;

            // ═══════════════════════════════════════════════════════════════
            // CASE 1: THEME MODE
            // Theme is provided → Presentation comes from THEME ONLY
            // User's Colors, Hero, Sections from request if not then from theme
            // ═══════════════════════════════════════════════════════════════

            Theme? theme = null;
            var hasThemeCode = !string.IsNullOrWhiteSpace(request.ThemeCode);

            if (hasThemeCode)
            {
                theme = await _themeRepository.GetByCodeAsync(request.ThemeCode);
            }

            var isValidActiveTheme =
                hasThemeCode &&
                theme != null &&
                theme.IsActive;

            if (isValidActiveTheme)
            {     
             
                // Prepare Sections => first from request , second from theme. 
                var sections = request.Sections != null && request.Sections.Any()
                ? request.Sections.Select(s => new SectionItem
                {
                    Id = s.Id,
                    Enabled = s.Enabled,
                    Order = s.Order
                }).ToList()
                : theme.Config.Sections.Select(s => new SectionItem
                {
                    Id = s.Id,
                    Enabled = s.Enabled,
                    Order = s.Order
                }).ToList();



                // Create TenantWebsite with Theme mode
                // NOTE: User's presentation input:
                // - Colors & Hero: IGNORED
                // - Sections: used if provided, otherwise fallback to theme

                tenantWebsite = new TenantWebsite
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Mode = WebsiteMode.Theme,
                    ThemeId = theme.Id,
                    IsPublished = true,
                    Config = new SiteConfig
                    {
                        // Business data: FROM USER
                        SiteName = request.SiteName,
                        Domain = request.Domain,
                        BusinessType = request.BusinessType,
                        LogoUrl = request.LogoUrl,
                        about_the_site = request.about_the_site,
                        location = request.location,
                        phone = request.phone,
                        email = request.email,

                        // Presentation data: FROM THEME (snapshot copy)
                        // User presentation input is IGNORED - theme always wins
                        Colors = new ThemeColors
                        {
                            Primary = theme.Config.Colors.Primary,
                            Secondary = theme.Config.Colors.Secondary,
                            Background = theme.Config.Colors.Background,
                            Text = theme.Config.Colors.Text
                        },
                        Hero = new HeroSection
                        {
                            Title = theme.Config.Hero.Title,
                            Subtitle = theme.Config.Hero.Subtitle,
                            ButtonText = theme.Config.Hero.ButtonText,
                            BackgroundImage = theme.Config.Hero.BackgroundImage
                        },
                        Sections = sections

                
                    }
                };


            }



            // ═══════════════════════════════════════════════════════════════
            // CASE 2: CUSTOM MODE
            // No theme → Presentation MUST come from USER
            // NO backend defaults - validation error if missing
            // ═══════════════════════════════════════════════════════════════
          
            
            else
            {
                // Create TenantWebsite with Custom mode
                // ALL presentation data comes from user - NO defaults
                tenantWebsite = new TenantWebsite
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Mode = WebsiteMode.Custom,
                    ThemeId = null,
                    IsPublished = true,
                    Config = new SiteConfig
                    {
                        // Business data: FROM USER
                        SiteName = request.SiteName,
                        Domain = request.Domain,
                        BusinessType = request.BusinessType,
                        LogoUrl = request.LogoUrl,
                        about_the_site = request.about_the_site,
                        location = request.location,
                        phone = request.phone,
                        email = request.email,
                        
                        // Presentation data: FROM USER (no defaults)
                        Colors = new ThemeColors
                        {
                            Primary = request.Colors.Primary,
                            Secondary = request.Colors.Secondary,
                            Background = request.Colors.Background,
                            Text = request.Colors.Text
                        },
                        Hero = new HeroSection
                        {
                            Title = request.Hero.Title,
                            Subtitle = request.Hero.Subtitle ?? string.Empty,
                            ButtonText = request.Hero.ButtonText,
                            BackgroundImage = request.Hero.BackgroundImage ?? string.Empty
                        },
                        Sections = request.Sections.Select(s => new SectionItem
                        {
                            Id = s.Id,
                            Enabled = s.Enabled,
                            Order = s.Order
                        }).ToList()
                    }
                };
            }

            // Persist
            await _tenantWebsiteRepository.CreateAsync(tenantWebsite);
            await _unitOfWork.SaveChangesAsync();

            return new WebsiteProvisioningResult { Success = true };
        }

        private static WebsiteProvisioningResult Fail(string error)
        {
            return new WebsiteProvisioningResult { Success = false, Error = error };
        }
    }
}
