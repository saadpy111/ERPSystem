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
    /// Supports rich text styling (including MarginTop), image styling, and
    /// full rich SectionItem deep-copy matching HeroSection behaviour.
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
            _themeRepository         = themeRepository;
            _tenantWebsiteRepository = tenantWebsiteRepository;
            _unitOfWork              = unitOfWork;
        }

        public async Task<ApplyThemeResponse> Handle(ApplyThemeCommand request, CancellationToken cancellationToken)
        {
            // Step 1: Load and validate theme
            var theme = await _themeRepository.GetByIdAsync(request.ThemeId);

            if (theme == null)
            {
                return new ApplyThemeResponse { Success = false, Error = "Theme not found" };
            }

            if (!theme.IsActive)
            {
                return new ApplyThemeResponse { Success = false, Error = "Theme is not active" };
            }

            // Step 2: Get or create tenant website
            var tenantWebsite = await _tenantWebsiteRepository.GetByTenantIdAsync(request.TenantId);

            if (tenantWebsite == null)
            {
                tenantWebsite = new Domain.Entities.TenantWebsite
                {
                    Id       = Guid.NewGuid(),
                    TenantId = request.TenantId,
                    Mode     = WebsiteMode.Theme,
                    ThemeId  = theme.Id,
                    Config   = new SiteConfig
                    {
                        // Business data starts empty – never sourced from the theme
                        SiteName       = string.Empty,
                        Domain         = string.Empty,
                        BusinessType   = string.Empty,
                        LogoUrl        = string.Empty,
                        about_the_site = string.Empty,
                        location       = string.Empty,
                        phone          = string.Empty,
                        email          = string.Empty,

                        // Presentation snapshot from theme
                        Colors          = SnapshotColors(theme.Config?.Colors),
                        Hero            = SnapshotHero(theme.Config?.Hero),
                        Sections        = SnapshotSections(theme.Config?.Sections),
                        ContactUsImages = SnapshotContactUsImages(theme.Config?.ContactUsImages)
                    },
                    IsPublished = false
                };

                await _tenantWebsiteRepository.CreateAsync(tenantWebsite);
            }
            else
            {
                tenantWebsite.Mode    = WebsiteMode.Theme;
                tenantWebsite.ThemeId = theme.Id;


                if (theme.Config != null)
                {
                    tenantWebsite.Config.Colors = SnapshotColors(theme.Config?.Colors);
                    tenantWebsite.Config.Hero = SnapshotHero(theme.Config?.Hero);
                    tenantWebsite.Config.Sections = SnapshotSections(theme.Config?.Sections);
                    tenantWebsite.Config.ContactUsImages = SnapshotContactUsImages(theme.Config?.ContactUsImages);
                }
                await _tenantWebsiteRepository.UpdateAsync(tenantWebsite);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ApplyThemeResponse { Success = true };
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Merge Helpers (Patch-like Application of Theme to Tenant overrides)
        // ─────────────────────────────────────────────────────────────────────

        private static void EnsureConfigStructure(Domain.Entities.TenantWebsite site)
        {
            site.Config ??= new SiteConfig();
            site.Config.Colors ??= new ThemeColors();
            site.Config.Hero ??= new HeroSection();

            site.Config.Hero.Title       ??= new TextContent();
            site.Config.Hero.Subtitle    ??= new TextContent();
            site.Config.Hero.ButtonText  ??= new TextContent();

            site.Config.Hero.Title.Style       ??= new TextStyle();
            site.Config.Hero.Subtitle.Style    ??= new TextStyle();
            site.Config.Hero.ButtonText.Style  ??= new TextStyle();

            site.Config.Hero.BackgroundImage       ??= new ImageContent();
            site.Config.Hero.BackgroundImage.Style ??= new ImageStyle();

            site.Config.ContactUsImages ??= new ContactUsImages();
            site.Config.ContactUsImages.ContactUsImg       ??= new ImageContent();
            site.Config.ContactUsImages.ContactUsImg.Style ??= new ImageStyle();
            site.Config.ContactUsImages.ClientOImg         ??= new ImageContent();
            site.Config.ContactUsImages.ClientOImg.Style   ??= new ImageStyle();

            site.Config.Sections ??= new List<SectionItem>();
            foreach (var section in site.Config.Sections)
            {
                section.Title           ??= new TextContent();
                section.Subtitle        ??= new TextContent();
                section.ButtonText      ??= new TextContent();
                section.BackgroundImage ??= new ImageContent();

                section.Title.Style           ??= new TextStyle();
                section.Subtitle.Style        ??= new TextStyle();
                section.ButtonText.Style      ??= new TextStyle();
                section.BackgroundImage.Style ??= new ImageStyle();
            }
        }

        private static void MergeColors(ThemeColors target, ThemeColors? source)
        {
            if (source == null) return;
            if (!string.IsNullOrEmpty(source.Primary)) target.Primary = source.Primary;
            if (!string.IsNullOrEmpty(source.Secondary)) target.Secondary = source.Secondary;
            if (!string.IsNullOrEmpty(source.Background)) target.Background = source.Background;
            if (!string.IsNullOrEmpty(source.Text)) target.Text = source.Text;
            if (!string.IsNullOrEmpty(source.FontFamily)) target.FontFamily = source.FontFamily;
        }

        private static void MergeHero(HeroSection target, HeroSection? source)
        {
            if (source == null) return;
            MergeTextContent(target.Title, source.Title);
            MergeTextContent(target.Subtitle, source.Subtitle);
            MergeTextContent(target.ButtonText, source.ButtonText);
            MergeImageContent(target.BackgroundImage, source.BackgroundImage);
        }

        private static void MergeSections(List<SectionItem> target, List<SectionItem>? source)
        {
            if (source == null) return;

            foreach (var themeSection in source)
            {
                var existing = target.FirstOrDefault(s => s.Id == themeSection.Id);
                if (existing == null)
                {
                    target.Add(SnapshotSectionItem(themeSection));
                }
                else
                {
                    // Merge fields; DO NOT override Enabled or Order
                    MergeTextContent(existing.Title, themeSection.Title);
                    MergeTextContent(existing.Subtitle, themeSection.Subtitle);
                    MergeTextContent(existing.ButtonText, themeSection.ButtonText);
                    MergeImageContent(existing.BackgroundImage, themeSection.BackgroundImage);
                }
            }
        }

        private static void MergeContactUsImages(ContactUsImages target, ContactUsImages? source)
        {
            if (source == null) return;
            MergeImageContent(target.ContactUsImg, source.ContactUsImg);
            MergeImageContent(target.ClientOImg, source.ClientOImg);
        }

        private static void MergeTextContent(TextContent target, TextContent? source)
        {
            if (source == null) return;

            // Only fill missing text to preserve user modifications
            if (string.IsNullOrEmpty(target.Text) && !string.IsNullOrEmpty(source.Text))
                target.Text = source.Text;

            MergeTextStyle(target.Style, source.Style);
        }

        private static void MergeTextStyle(TextStyle target, TextStyle? source)
        {
            if (source == null) return;

            // Copy styles from theme
            target.FontSize = source.FontSize;
            target.FontWeight = source.FontWeight;
            if (!string.IsNullOrEmpty(source.Color)) target.Color = source.Color;
            target.Alignment = source.Alignment;
            target.HorizontalSpacing = source.HorizontalSpacing;
            target.VerticalSpacing = source.VerticalSpacing;
            if (source.MarginTop.HasValue) target.MarginTop = source.MarginTop.Value;
            if (!string.IsNullOrEmpty(source.BackgroundColor)) target.BackgroundColor = source.BackgroundColor;
        }

        private static void MergeImageContent(ImageContent target, ImageContent? source)
        {
            if (source == null) return;

            if (!string.IsNullOrEmpty(source.Url))
                target.Url = source.Url;

            MergeImageStyle(target.Style, source.Style);
        }

        private static void MergeImageStyle(ImageStyle target, ImageStyle? source)
        {
            if (source == null) return;

            target.BorderRadius = source.BorderRadius;
            if (!string.IsNullOrEmpty(source.OverlayColor)) target.OverlayColor = source.OverlayColor;
            target.OverlayOpacity = source.OverlayOpacity;
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Snapshot helpers
        //  These produce a deep-copy of the theme's presentation data so that
        //  future theme edits do NOT retroactively affect tenant configs.
        // ─────────────────────────────────────────────────────────────────────

        private static ThemeColors SnapshotColors(ThemeColors? src) => new()
        {
            Primary    = src?.Primary    ?? "#000000",
            Secondary  = src?.Secondary  ?? "#ffffff",
            Background = src?.Background ?? "#ffffff",
            Text       = src?.Text       ?? "#000000",
            FontFamily = src?.FontFamily ?? "Default"
        };

        private static HeroSection SnapshotHero(HeroSection? src) => new()
        {
            Title           = SnapshotTextContent(src?.Title),
            Subtitle        = SnapshotTextContent(src?.Subtitle),
            ButtonText      = SnapshotTextContent(src?.ButtonText),
            BackgroundImage = SnapshotImageContent(src?.BackgroundImage)
        };

        /// <summary>
        /// Deep-copies a list of SectionItems, now including full rich fields:
        /// Title, Subtitle, ButtonText, BackgroundImage — matching Hero behaviour.
        /// </summary>
        private static List<SectionItem> SnapshotSections(List<SectionItem>? src)
            => (src ?? new()).Select(SnapshotSectionItem).ToList();

        private static SectionItem SnapshotSectionItem(SectionItem? src) => new()
        {
            Id              = src?.Id      ?? string.Empty,
            Enabled         = src?.Enabled ?? true,
            Order           = src?.Order   ?? 0,
            Title           = SnapshotTextContent(src?.Title),
            Subtitle        = SnapshotTextContent(src?.Subtitle),
            ButtonText      = SnapshotTextContent(src?.ButtonText),
            BackgroundImage = SnapshotImageContent(src?.BackgroundImage)
        };

        private static TextContent SnapshotTextContent(TextContent? src) => new()
        {
            Text  = src?.Text ?? string.Empty,
            Style = new TextStyle
            {
                FontSize          = src?.Style?.FontSize          ?? 16,
                FontWeight        = src?.Style?.FontWeight        ?? FontWeight.Normal,
                Color             = src?.Style?.Color             ?? "#000000",
                Alignment         = src?.Style?.Alignment         ?? TextAlign.Left,
                HorizontalSpacing = src?.Style?.HorizontalSpacing ?? 0,
                VerticalSpacing   = src?.Style?.VerticalSpacing   ?? 0,
                MarginTop         = src?.Style?.MarginTop         ?? 0,   // preserved in snapshot
                BackgroundColor   = src?.Style?.BackgroundColor
            }
        };
        private static void NormalizeConfig(SiteConfig config)
        {
            config ??= new SiteConfig();

            // Colors
            config.Colors ??= new ThemeColors
            {
                Primary = "#000000",
                Secondary = "#ffffff",
                Background = "#ffffff",
                Text = "#000000",
                FontFamily = "Default"
            };

            // Hero
            config.Hero ??= new HeroSection();
            config.Hero.Title = SnapshotTextContent(config.Hero.Title);
            config.Hero.Subtitle = SnapshotTextContent(config.Hero.Subtitle);
            config.Hero.ButtonText = SnapshotTextContent(config.Hero.ButtonText);
            config.Hero.BackgroundImage = SnapshotImageContent(config.Hero.BackgroundImage);

            // ContactUs
            config.ContactUsImages ??= new ContactUsImages();
            config.ContactUsImages.ContactUsImg = SnapshotImageContent(config.ContactUsImages.ContactUsImg);
            config.ContactUsImages.ClientOImg = SnapshotImageContent(config.ContactUsImages.ClientOImg);

            // Sections
            config.Sections ??= new List<SectionItem>();

            for (int i = 0; i < config.Sections.Count; i++)
            {
                var s = config.Sections[i] ?? new SectionItem();

                config.Sections[i] = new SectionItem
                {
                    Id = string.IsNullOrWhiteSpace(s.Id) ? Guid.NewGuid().ToString() : s.Id,
                    Enabled = s.Enabled,
                    Order = s.Order,

                    Title = SnapshotTextContent(s.Title),
                    Subtitle = SnapshotTextContent(s.Subtitle),
                    ButtonText = SnapshotTextContent(s.ButtonText),
                    BackgroundImage = SnapshotImageContent(s.BackgroundImage)
                };
            }
        }
        private static ImageContent SnapshotImageContent(ImageContent? src) => new()
        {
            Url   = src?.Url ?? string.Empty,
            Style = new ImageStyle
            {
                BorderRadius   = src?.Style?.BorderRadius   ?? 6,
                OverlayColor   = src?.Style?.OverlayColor   ?? "#FFFFFF",
                OverlayOpacity = src?.Style?.OverlayOpacity ?? 40
            }
        };

        private static ContactUsImages SnapshotContactUsImages(ContactUsImages? src) => new()
        {
            ContactUsImg = SnapshotImageContent(src?.ContactUsImg),
            ClientOImg   = SnapshotImageContent(src?.ClientOImg)
        };
    }
}