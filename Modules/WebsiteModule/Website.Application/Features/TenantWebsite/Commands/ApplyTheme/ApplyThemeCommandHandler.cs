using MediatR;
using Website.Application.Contracts.Persistence;
using Website.Domain.Enums;
using Website.Domain.ValueObjects;

namespace Website.Application.Features.TenantWebsite.Commands.ApplyTheme
{
    /// <summary>
    /// Handler for applying a theme to a tenant's website.
    /// CRITICAL: Theme is COPIED into SiteConfig (snapshot), not live-linked.
    /// Business data is NEVER overridden.
    /// </summary>
    public class ApplyThemeCommandHandler : IRequestHandler<ApplyThemeCommand, ApplyThemeResponse>
    {
        private readonly IThemeRepository _themeRepository;
        private readonly ITenantWebsiteRepository _tenantWebsiteRepository;
        private readonly IWebsiteUnitOfWork _unitOfWork;

        public ApplyThemeCommandHandler(
            IThemeRepository themeRepository,
            ITenantWebsiteRepository tenantWebsiteRepository,
            IWebsiteUnitOfWork unitOfWork)
        {
            _themeRepository = themeRepository;
            _tenantWebsiteRepository = tenantWebsiteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApplyThemeResponse> Handle(ApplyThemeCommand request, CancellationToken cancellationToken)
        {
            // Step 1: Load and validate theme
            var theme = await _themeRepository.GetByIdAsync(request.ThemeId);
            
            if (theme == null)
            {
                return new ApplyThemeResponse
                {
                    Success = false,
                    Error = "Theme not found"
                };
            }

            if (!theme.IsActive)
            {
                return new ApplyThemeResponse
                {
                    Success = false,
                    Error = "Theme is not active"
                };
            }

            // Step 2: Get or create tenant website
            var tenantWebsite = await _tenantWebsiteRepository.GetByTenantIdAsync(request.TenantId);
            
            if (tenantWebsite == null)
            {
                // Create new tenant website with theme
                tenantWebsite = new Domain.Entities.TenantWebsite
                {
                    Id = Guid.NewGuid(),
                    TenantId = request.TenantId,
                    Mode = WebsiteMode.Theme,
                    ThemeId = theme.Id,
                    Config = new SiteConfig
                    {
                        // Business data starts empty (tenant must provide)
                        SiteName = string.Empty,
                        Domain = string.Empty,
                        BusinessType = string.Empty,
                        LogoUrl = string.Empty,
                        
                        // Presentation data COPIED from theme (SNAPSHOT)
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
                    },
                    IsPublished = false
                };

                await _tenantWebsiteRepository.CreateAsync(tenantWebsite);
            }
            else
            {
                // PRESERVE business data, replace ONLY presentation data
                var existingConfig = tenantWebsite.Config;
                
                tenantWebsite.Mode = WebsiteMode.Theme;
                tenantWebsite.ThemeId = theme.Id;
                
                // Copy ONLY presentation data (Colors, Hero, Sections) - SNAPSHOT
                tenantWebsite.Config.Colors = new ThemeColors
                {
                    Primary = theme.Config.Colors.Primary,
                    Secondary = theme.Config.Colors.Secondary,
                    Background = theme.Config.Colors.Background,
                    Text = theme.Config.Colors.Text
                };
                tenantWebsite.Config.Hero = new HeroSection
                {
                    Title = theme.Config.Hero.Title,
                    Subtitle = theme.Config.Hero.Subtitle,
                    ButtonText = theme.Config.Hero.ButtonText,
                    BackgroundImage = theme.Config.Hero.BackgroundImage
                };
                tenantWebsite.Config.Sections = theme.Config.Sections.Select(s => new SectionItem
                {
                    Id = s.Id,
                    Enabled = s.Enabled,
                    Order = s.Order
                }).ToList();

                // Business data (SiteName, Domain, BusinessType, LogoUrl) PRESERVED
                // They were NOT touched above

                await _tenantWebsiteRepository.UpdateAsync(tenantWebsite);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ApplyThemeResponse { Success = true };
        }
    }
}
