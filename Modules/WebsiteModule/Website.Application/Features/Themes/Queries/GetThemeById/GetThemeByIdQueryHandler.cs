using MediatR;
using Website.Application.Contracts.Persistence;
using Website.Application.DTOs;
using Website.Domain.ValueObjects;
using SharedKernel.Core.Files;

namespace Website.Application.Features.Themes.Queries.GetThemeById
{
    public class GetThemeByIdQueryHandler : IRequestHandler<GetThemeByIdQuery, GetThemeByIdResponse>
    {
        private readonly IThemeRepository _themeRepository;
        private readonly IFileUrlResolver _fileUrlResolver;

        public GetThemeByIdQueryHandler(
            IThemeRepository themeRepository,
            IFileUrlResolver fileUrlResolver)
        {
            _themeRepository = themeRepository;
            _fileUrlResolver = fileUrlResolver;
        }

        public async Task<GetThemeByIdResponse> Handle(GetThemeByIdQuery request, CancellationToken cancellationToken)
        {
            var theme = await _themeRepository.GetByIdAsync(request.ThemeId);

            if (theme == null)
            {
                return new GetThemeByIdResponse
                {
                    Success = false,
                    Error = "Theme not found"
                };
            }

            return new GetThemeByIdResponse
            {
                Success = true,
                Theme = MapThemeDto(theme)
            };
        }

        private ThemeDto MapThemeDto(Domain.Entities.Theme theme)
        {
            var config = theme.Config ?? new ThemeConfig();

            var hero = config.Hero ?? new HeroSection();

            var title = hero.Title ?? new TextContent();
            var subtitle = hero.Subtitle ?? new TextContent();
            var buttonText = hero.ButtonText ?? new TextContent();

            var bgImage = hero.BackgroundImage ?? new ImageContent();

            var colors = config.Colors ?? new ThemeColors();

            return new ThemeDto
            {
                Id = theme.Id,
                Code = theme.Code,
                Name = theme.Name,

                PreviewImage =
                    _fileUrlResolver.Resolve(theme.PreviewImage)
                    ?? theme.PreviewImage,

                IsActive = theme.IsActive,

                Config = new ThemeConfig
                {
                    Colors = new ThemeColors
                    {
                        Primary = colors.Primary,
                        Secondary = colors.Secondary,
                        Background = colors.Background,
                        Text = colors.Text,
                        FontFamily = colors.FontFamily
                    },

                    Hero = new HeroSection
                    {
                        Title = MapTextContent(title),
                        Subtitle = MapTextContent(subtitle),
                        ButtonText = MapTextContent(buttonText),

                        BackgroundImage = new ImageContent
                        {
                            Url =
                                _fileUrlResolver.Resolve(bgImage.Url)
                                ?? bgImage.Url
                                ?? string.Empty,

                            Style = new ImageStyle
                            {
                                BorderRadius = bgImage.Style?.BorderRadius ?? 6,
                                OverlayColor = bgImage.Style?.OverlayColor ?? "#FFFFFF",
                                OverlayOpacity = bgImage.Style?.OverlayOpacity ?? 40
                            }
                        }
                    },

                    Sections = config.Sections?
                        .Select(s => new SectionItem
                        {
                            Id = s.Id,
                            Enabled = s.Enabled,
                            Order = s.Order
                        }).ToList() ?? new()
                },

                CreatedAt = theme.CreatedAt,
                UpdatedAt = theme.UpdatedAt
            };
        }

        private static TextContent MapTextContent(TextContent src)
        {
            var style = src.Style ?? new TextStyle();

            return new TextContent
            {
                Text = src.Text,
                Style = new TextStyle
                {
                    FontSize = style.FontSize == 0 ? 16 : style.FontSize,
                    FontWeight = style.FontWeight,
                    Color = style.Color ?? "#000000",
                    Alignment = style.Alignment,
                    HorizontalSpacing = style.HorizontalSpacing,
                    VerticalSpacing = style.VerticalSpacing
                }
            };
        }
    }
}