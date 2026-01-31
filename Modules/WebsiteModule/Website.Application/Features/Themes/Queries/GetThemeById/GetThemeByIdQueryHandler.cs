using MediatR;
using Website.Application.Contracts.Persistence;
using Website.Application.DTOs;
using SharedKernel.Core.Files;

namespace Website.Application.Features.Themes.Queries.GetThemeById
{
    public class GetThemeByIdQueryHandler : IRequestHandler<GetThemeByIdQuery, GetThemeByIdResponse>
    {
        private readonly IThemeRepository _themeRepository;
        private readonly IFileUrlResolver _fileUrlResolver;

        public GetThemeByIdQueryHandler(IThemeRepository themeRepository, IFileUrlResolver fileUrlResolver)
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
                Theme = new ThemeDto
                {
                    Id = theme.Id,
                    Code = theme.Code,
                    Name = theme.Name,
                    PreviewImage = _fileUrlResolver.Resolve(theme.PreviewImage) ?? theme.PreviewImage,
                    IsActive = theme.IsActive,
                    Config = new Domain.ValueObjects.ThemeConfig
                    {
                        Colors = theme.Config.Colors,
                        Hero = new Domain.ValueObjects.HeroSection
                        {
                            Title = theme.Config.Hero.Title,
                            Subtitle = theme.Config.Hero.Subtitle,
                            ButtonText = theme.Config.Hero.ButtonText,
                            BackgroundImage = _fileUrlResolver.Resolve(theme.Config.Hero.BackgroundImage) ?? theme.Config.Hero.BackgroundImage
                        },
                        Sections = theme.Config.Sections
                    },
                    CreatedAt = theme.CreatedAt,
                    UpdatedAt = theme.UpdatedAt
                }
            };
        }
    }
}
