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
    /// 
    /// ═══════════════════════════════════════════════════════════════
    /// CASE 1: THEME MODE (ThemeCode is provided)
    /// ═══════════════════════════════════════════════════════════════
    /// - Business data (SiteName, Domain, BusinessType, LogoUrl): FROM USER
    /// - Presentation data (Colors, Hero, Sections): FROM THEME ONLY
    /// - User presentation input is COMPLETELY IGNORED
    /// - Theme data ALWAYS wins - NO merging, NO fallback
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
            // ===== VALIDATION: Business data is ALWAYS required =====
            if (string.IsNullOrWhiteSpace(request.SiteName))
                return Fail("SiteName is required");
            
            if (string.IsNullOrWhiteSpace(request.Domain))
                return Fail("Domain is required");
            
            if (string.IsNullOrWhiteSpace(request.BusinessType))
                return Fail("BusinessType is required");
            
            if (string.IsNullOrWhiteSpace(request.LogoUrl))
                return Fail("LogoUrl is required");

            // Check if tenant already has a website
            if (await _tenantWebsiteRepository.ExistsAsync(tenantId))
                return Fail("Tenant website already exists");

            TenantWebsite tenantWebsite;

            // ═══════════════════════════════════════════════════════════════
            // CASE 1: THEME MODE
            // Theme is provided → Presentation comes from THEME ONLY
            // User's Colors, Hero, Sections are COMPLETELY IGNORED
            // ═══════════════════════════════════════════════════════════════
            if (!string.IsNullOrWhiteSpace(request.ThemeCode))
            {
                var theme = await _themeRepository.GetByCodeAsync(request.ThemeCode);
                
                if (theme == null)
                    return Fail($"Theme '{request.ThemeCode}' not found");

                if (!theme.IsActive)
                    return Fail($"Theme '{request.ThemeCode}' is not active");

                // Create TenantWebsite with Theme mode
                // NOTE: User's presentation input (Colors, Hero, Sections) is IGNORED
                // Theme is the ONLY source of truth for presentation
                tenantWebsite = new TenantWebsite
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Mode = WebsiteMode.Theme,
                    ThemeId = theme.Id,
                    IsPublished = false,
                    Config = new SiteConfig
                    {
                        // Business data: FROM USER
                        SiteName = request.SiteName,
                        Domain = request.Domain,
                        BusinessType = request.BusinessType,
                        LogoUrl = request.LogoUrl,
                        
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
                        Sections = theme.Config.Sections.Select(s => new SectionItem
                        {
                            Id = s.Id,
                            Enabled = s.Enabled,
                            Order = s.Order
                        }).ToList()
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
                // STRICT VALIDATION: All presentation data is REQUIRED
                // We do NOT create default UI values - that's the frontend/user's job
                
                if (request.Colors == null)
                    return Fail("Colors configuration is required for custom website mode");
                
                if (request.Hero == null)
                    return Fail("Hero configuration is required for custom website mode");
                
                if (request.Sections == null || request.Sections.Count == 0)
                    return Fail("At least one section is required for custom website mode");

                // Validate Colors has actual values
                if (string.IsNullOrWhiteSpace(request.Colors.Primary))
                    return Fail("Colors.Primary is required");
                if (string.IsNullOrWhiteSpace(request.Colors.Secondary))
                    return Fail("Colors.Secondary is required");
                if (string.IsNullOrWhiteSpace(request.Colors.Background))
                    return Fail("Colors.Background is required");
                if (string.IsNullOrWhiteSpace(request.Colors.Text))
                    return Fail("Colors.Text is required");

                // Validate Hero has actual values
                if (string.IsNullOrWhiteSpace(request.Hero.Title))
                    return Fail("Hero.Title is required");
                if (string.IsNullOrWhiteSpace(request.Hero.ButtonText))
                    return Fail("Hero.ButtonText is required");
                if (string.IsNullOrWhiteSpace(request.Hero.BackgroundImage))
                    return Fail("Hero.BackgroundImage is required for custom website mode");

                // Create TenantWebsite with Custom mode
                // ALL presentation data comes from user - NO defaults
                tenantWebsite = new TenantWebsite
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Mode = WebsiteMode.Custom,
                    ThemeId = null,
                    IsPublished = false,
                    Config = new SiteConfig
                    {
                        // Business data: FROM USER
                        SiteName = request.SiteName,
                        Domain = request.Domain,
                        BusinessType = request.BusinessType,
                        LogoUrl = request.LogoUrl,
                        
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
