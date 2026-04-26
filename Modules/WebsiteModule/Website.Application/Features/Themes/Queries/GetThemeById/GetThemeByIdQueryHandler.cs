using MediatR;
using Website.Application.Contracts.Persistence;
using Website.Application.DTOs;
using Website.Application.Mappers;

namespace Website.Application.Features.Themes.Queries.GetThemeById
{
    public class GetThemeByIdQueryHandler : IRequestHandler<GetThemeByIdQuery, GetThemeByIdResponse>
    {
        private readonly IThemeRepository _themeRepository;
        private readonly IWebsiteConfigMapper _mapper;

        public GetThemeByIdQueryHandler(
            IThemeRepository themeRepository,
            IWebsiteConfigMapper mapper)
        {
            _themeRepository = themeRepository;
            _mapper          = mapper;
        }

        public async Task<GetThemeByIdResponse> Handle(GetThemeByIdQuery request, CancellationToken cancellationToken)
        {
            var theme = await _themeRepository.GetByIdAsync(request.ThemeId);

            if (theme == null)
            {
                return new GetThemeByIdResponse
                {
                    Success = false,
                    Error   = "Theme not found"
                };
            }

            return new GetThemeByIdResponse
            {
                Success = true,
                Theme   = _mapper.MapThemeDto(theme)
            };
        }
    }
}