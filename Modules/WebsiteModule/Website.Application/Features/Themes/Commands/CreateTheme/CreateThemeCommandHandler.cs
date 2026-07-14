using MediatR;
using Website.Application.Contracts.Infrastruture;
using Website.Application.Contracts.Persistence;
using Website.Domain.Entities;
using Website.Domain.Enums;
using Website.Domain.ValueObjects;

namespace Website.Application.Features.Themes.Commands.CreateTheme
{
    public class CreateThemeCommandHandler : IRequestHandler<CreateThemeCommand, CreateThemeResponse>
    {
        private readonly IThemeRepository _themeRepository;
        private readonly IWebsiteUnitOfWork _unitOfWork;
        private readonly IWebsiteImageService _websiteImageService;

        public CreateThemeCommandHandler(
            IThemeRepository themeRepository,
            IWebsiteUnitOfWork unitOfWork,
            IWebsiteImageService websiteImageService)
        {
            _themeRepository = themeRepository;
            _unitOfWork = unitOfWork;
            _websiteImageService = websiteImageService;
        }

        public async Task<CreateThemeResponse> Handle(CreateThemeCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
            {
                return new CreateThemeResponse
                {
                    Success = false,
                    Error = "Theme code is required"
                };
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new CreateThemeResponse
                {
                    Success = false,
                    Error = "Theme name is required"
                };
            }

            var code = request.Code.Trim().ToLowerInvariant();

            if (await _themeRepository.ExistsAsync(code))
            {
                return new CreateThemeResponse
                {
                    Success = false,
                    Error = $"Theme with code '{code}' already exists"
                };
            }

            string previewImagePath = string.Empty;
            string heroBackgroundImagePath = string.Empty;

            if (request.PreviewImageFile != null)
            {
                previewImagePath = await _websiteImageService
                    .ProcessThemePreviewImageAsync(code, request.PreviewImageFile);
            }

            if (request.HeroBackgroundImageFile != null)
            {
                heroBackgroundImagePath = await _websiteImageService
                    .ProcessThemeHeroImageAsync(code, request.HeroBackgroundImageFile);
            }

            var theme = new Theme
            {
                Id = Guid.NewGuid(),
                Code = code,
                Name = request.Name,
                PreviewImage = previewImagePath,
                IsActive = request.IsActive,
                Config = new ThemeConfig
                {
                    Colors = new ThemeColors
                    {
                        Primary = request.PrimaryColor ?? string.Empty,
                        Secondary = request.SecondaryColor ?? string.Empty,
                        Background = request.BackgroundColor ?? string.Empty,
                        Text = request.TextColor ?? string.Empty,
                        FontFamily = request.FontFamily ?? "Neo Sans Arabic"
                    },

                    Hero = new HeroSection
                    {
                        Title = BuildTextContent(
                            request.HeroTitle,
                            request.HeroTitleFontSize,
                            request.HeroTitleFontWeight,
                            request.HeroTitleColor,
                            request.HeroTitleAlignment,
                            request.HeroTitleHorizontalSpacing,
                            request.HeroTitleVerticalSpacing,
                            request.HeroTitleBackgroundColor),

                        Subtitle = BuildTextContent(
                            request.HeroSubtitle,
                            request.HeroSubtitleFontSize,
                            request.HeroSubtitleFontWeight,
                            request.HeroSubtitleColor,
                            request.HeroSubtitleAlignment,
                            request.HeroSubtitleHorizontalSpacing,
                            request.HeroSubtitleVerticalSpacing,
                            request.HeroSubtitleBackgroundColor),

                        ButtonText = BuildTextContent(
                            request.HeroButtonText,
                            request.HeroButtonTextFontSize,
                            request.HeroButtonTextFontWeight,
                            request.HeroButtonTextColor,
                            request.HeroButtonTextAlignment,
                            request.HeroButtonTextHorizontalSpacing,
                            request.HeroButtonTextVerticalSpacing,
                            request.HeroButtonTextBackgroundColor),

                        BackgroundImage = new ImageContent
                        {
                            Url = heroBackgroundImagePath,
                            Style = new ImageStyle
                            {
                                BorderRadius = request.HeroBackgroundBorderRadius ?? 6,
                                OverlayColor = request.HeroBackgroundOverlayColor ?? "#FFFFFF",
                                OverlayOpacity = request.HeroBackgroundOverlayOpacity ?? 40
                            }
                        }
                    },

                    Sections = request.Sections?.Select(s => new SectionItem
                    {
                        Id = s.Id,
                        Enabled = s.Enabled,
                        Order = s.Order
                    }).ToList() ?? new List<SectionItem>()
                }
            };

            await _themeRepository.CreateAsync(theme);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateThemeResponse
            {
                Success = true,
                ThemeId = theme.Id
            };
        }

        private static TextContent BuildTextContent(
            string? text,
            int? fontSize,
            FontWeight? weight,
            string? color,
            TextAlign? align,
            int? horizontalSpacing,
            int? verticalSpacing,
            string? backgroundColor = null)
        {
            return new TextContent
            {
                Text = text ?? string.Empty,
                Style = new TextStyle
                {
                    FontSize = fontSize ?? 16,
                    FontWeight = weight ?? FontWeight.Normal,
                    Color = color ?? "#000000",
                    Alignment = align ?? TextAlign.Left,
                    HorizontalSpacing = horizontalSpacing ?? 0,
                    VerticalSpacing = verticalSpacing ?? 0,
                    BackgroundColor = backgroundColor
                }
            };
        }
    }
}