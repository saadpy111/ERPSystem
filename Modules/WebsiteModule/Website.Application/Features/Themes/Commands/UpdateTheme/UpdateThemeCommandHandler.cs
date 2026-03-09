using MediatR;
using SharedKernel.Website;
using Website.Application.Contracts.Infrastruture.FileService;
using Website.Application.Contracts.Persistence;
using Website.Domain.Entities;
using Website.Domain.Enums;
using Website.Domain.ValueObjects;

namespace Website.Application.Features.Themes.Commands.UpdateTheme
{
    public class UpdateThemeCommandHandler : IRequestHandler<UpdateThemeCommand, UpdateThemeResponse>
    {
        private readonly IThemeRepository _themeRepository;
        private readonly IWebsiteUnitOfWork _unitOfWork;
        private readonly IWebsiteImageService _websiteImageService;
        private readonly IFileService _fileService;

        public UpdateThemeCommandHandler(
            IThemeRepository themeRepository,
            IWebsiteUnitOfWork unitOfWork,
            IWebsiteImageService websiteImageService,
            IFileService fileService)
        {
            _themeRepository = themeRepository;
            _unitOfWork = unitOfWork;
            _websiteImageService = websiteImageService;
            _fileService = fileService;
        }

        public async Task<UpdateThemeResponse> Handle(UpdateThemeCommand request, CancellationToken cancellationToken)
        {
            var theme = await _themeRepository.GetByIdAsync(request.ThemeId);

            if (theme == null)
            {
                return new UpdateThemeResponse
                {
                    Success = false,
                    Error = "Theme not found"
                };
            }

            EnsureConfigStructure(theme);

            theme.Name = request.Name;
            theme.IsActive = request.IsActive;

            // ───────── Preview Image ─────────
            if (request.PreviewImageFile != null)
            {
                if (!string.IsNullOrEmpty(theme.PreviewImage))
                    await _fileService.DeleteFileAsync(theme.PreviewImage);

                theme.PreviewImage =
                    await _websiteImageService.ProcessThemePreviewImageAsync(
                        theme.Code,
                        request.PreviewImageFile);
            }

            // ───────── Hero Background Image ─────────
            if (request.HeroBackgroundImageFile != null)
            {
                if (!string.IsNullOrEmpty(theme.Config.Hero.BackgroundImage.Url))
                    await _fileService.DeleteFileAsync(theme.Config.Hero.BackgroundImage.Url);

                theme.Config.Hero.BackgroundImage.Url =
                    await _websiteImageService.ProcessThemeHeroImageAsync(
                        theme.Code,
                        request.HeroBackgroundImageFile);
            }

            // ───────── Colors ─────────
            if (request.PrimaryColor != null)
                theme.Config.Colors.Primary = request.PrimaryColor;

            if (request.SecondaryColor != null)
                theme.Config.Colors.Secondary = request.SecondaryColor;

            if (request.BackgroundColor != null)
                theme.Config.Colors.Background = request.BackgroundColor;

            if (request.TextColor != null)
                theme.Config.Colors.Text = request.TextColor;

            if (request.FontFamily != null)
                theme.Config.Colors.FontFamily = request.FontFamily;

            // ───────── Hero Title ─────────
            UpdateTextContent(
                theme.Config.Hero.Title,
                request.HeroTitle,
                request.HeroTitleFontSize,
                request.HeroTitleFontWeight,
                request.HeroTitleColor,
                request.HeroTitleAlignment,
                request.HeroTitleHorizontalSpacing,
                request.HeroTitleVerticalSpacing);

            // ───────── Hero Subtitle ─────────
            UpdateTextContent(
                theme.Config.Hero.Subtitle,
                request.HeroSubtitle,
                request.HeroSubtitleFontSize,
                request.HeroSubtitleFontWeight,
                request.HeroSubtitleColor,
                request.HeroSubtitleAlignment,
                request.HeroSubtitleHorizontalSpacing,
                request.HeroSubtitleVerticalSpacing);

            // ───────── Hero Button ─────────
            UpdateTextContent(
                theme.Config.Hero.ButtonText,
                request.HeroButtonText,
                request.HeroButtonTextFontSize,
                request.HeroButtonTextFontWeight,
                request.HeroButtonTextColor,
                request.HeroButtonTextAlignment,
                request.HeroButtonTextHorizontalSpacing,
                request.HeroButtonTextVerticalSpacing);

            // ───────── Hero Image Style ─────────
            if (request.HeroBackgroundBorderRadius.HasValue)
                theme.Config.Hero.BackgroundImage.Style.BorderRadius =
                    request.HeroBackgroundBorderRadius.Value;

            if (request.HeroBackgroundOverlayColor != null)
                theme.Config.Hero.BackgroundImage.Style.OverlayColor =
                    request.HeroBackgroundOverlayColor;

            if (request.HeroBackgroundOverlayOpacity.HasValue)
                theme.Config.Hero.BackgroundImage.Style.OverlayOpacity =
                    request.HeroBackgroundOverlayOpacity.Value;

            // ───────── Sections ─────────
            if (request.Sections != null)
            {
                theme.Config.Sections = request.Sections
                    .Select(s => new SectionItem
                    {
                        Id = s.Id,
                        Enabled = s.Enabled,
                        Order = s.Order
                    })
                    .ToList();
            }

            await _themeRepository.UpdateAsync(theme);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateThemeResponse { Success = true };
        }

        // ───────────────── Helpers ─────────────────

        private static void EnsureConfigStructure(Theme theme)
        {
            theme.Config ??= new ThemeConfig();
            theme.Config.Colors ??= new ThemeColors();
            theme.Config.Hero ??= new HeroSection();

            theme.Config.Hero.Title ??= DefaultTextContent();
            theme.Config.Hero.Subtitle ??= DefaultTextContent();
            theme.Config.Hero.ButtonText ??= DefaultTextContent();
            theme.Config.Hero.BackgroundImage ??= DefaultImageContent();
        }

        private static void UpdateTextContent(
            TextContent target,
            string? text,
            int? fontSize,
            FontWeight? weight,
            string? color,
            TextAlign? align,
            int? hSpacing,
            int? vSpacing)
        {
            if (text != null)
                target.Text = text;

            if (fontSize.HasValue)
                target.Style.FontSize = fontSize.Value;

            if (weight.HasValue)
                target.Style.FontWeight = weight.Value;

            if (color != null)
                target.Style.Color = color;

            if (align.HasValue)
                target.Style.Alignment = align.Value;

            if (hSpacing.HasValue)
                target.Style.HorizontalSpacing = hSpacing.Value;

            if (vSpacing.HasValue)
                target.Style.VerticalSpacing = vSpacing.Value;
        }

        private static TextContent DefaultTextContent()
        {
            return new TextContent
            {
                Text = "",
                Style = new TextStyle
                {
                    FontSize = 16,
                    FontWeight = FontWeight.Normal,
                    Color = "#000000",
                    Alignment = TextAlign.Left,
                    HorizontalSpacing = 0,
                    VerticalSpacing = 0
                }
            };
        }

        private static ImageContent DefaultImageContent()
        {
            return new ImageContent
            {
                Url = "",
                Style = new ImageStyle
                {
                    BorderRadius = 6,
                    OverlayColor = "#FFFFFF",
                    OverlayOpacity = 40
                }
            };
        }
    }
}