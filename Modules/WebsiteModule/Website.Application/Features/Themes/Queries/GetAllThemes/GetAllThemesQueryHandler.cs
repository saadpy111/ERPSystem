using MediatR;
using Website.Application.Contracts.Persistence;
using Website.Application.DTOs;
using Website.Application.Mappers;

namespace Website.Application.Features.Themes.Queries.GetAllThemes
{
    public class GetAllThemesQueryHandler : IRequestHandler<GetAllThemesQuery, GetAllThemesResponse>
    {
        private readonly IThemeRepository _themeRepository;
        private readonly IWebsiteConfigMapper _mapper;

        public GetAllThemesQueryHandler(
            IThemeRepository themeRepository,
            IWebsiteConfigMapper mapper)
        {
            _themeRepository = themeRepository;
            _mapper          = mapper;
        }

        public async Task<GetAllThemesResponse> Handle(GetAllThemesQuery request, CancellationToken cancellationToken)
        {
            var themes = await _themeRepository.GetAllAsync();

            var themeDtos = themes
                .Select(_mapper.MapThemeDto)
                .ToList();

            return new GetAllThemesResponse
            {
                Success = true,
                Themes  = themeDtos
            };
        }
    }
}