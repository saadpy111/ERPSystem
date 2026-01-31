using MediatR;
using Website.Application.Contracts.Persistence;
using Website.Application.DTOs;
using SharedKernel.Core.Files;

namespace Website.Application.Features.Themes.Queries.GetAllThemes
{
    public class GetAllThemesQueryHandler : IRequestHandler<GetAllThemesQuery, GetAllThemesResponse>
    {
        private readonly IThemeRepository _themeRepository;
        private readonly IFileUrlResolver _fileUrlResolver;

        public GetAllThemesQueryHandler(IThemeRepository themeRepository, IFileUrlResolver fileUrlResolver)
        {
            _themeRepository = themeRepository;
            _fileUrlResolver = fileUrlResolver;
        }

        public async Task<GetAllThemesResponse> Handle(GetAllThemesQuery request, CancellationToken cancellationToken)
        {
            var themes = await _themeRepository.GetAllAsync(request.ActiveOnly);

            var themeDtos = themes.Select(t => {
                var dto = new ThemeDto
                {
                    Id = t.Id,
                    Code = t.Code,
                    Name = t.Name,
                    PreviewImage = _fileUrlResolver.Resolve(t.PreviewImage) ?? t.PreviewImage,
                    IsActive = t.IsActive,
                    Config = new Domain.ValueObjects.ThemeConfig
                    {
                        Colors = t.Config.Colors,
                        Hero = new Domain.ValueObjects.HeroSection
                        {
                            Title = t.Config.Hero.Title,
                            Subtitle = t.Config.Hero.Subtitle,
                            ButtonText = t.Config.Hero.ButtonText,
                            BackgroundImage = _fileUrlResolver.Resolve(t.Config.Hero.BackgroundImage) ?? t.Config.Hero.BackgroundImage
                        },
                        Sections = t.Config.Sections
                    },
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                };
                return dto;
            }).ToList();

            return new GetAllThemesResponse
            {
                Success = true,
                Themes = themeDtos
            };
        }
    }
}
