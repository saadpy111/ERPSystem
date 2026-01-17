using MediatR;
using Website.Application.Contracts.Persistence;
using Website.Application.DTOs;

namespace Website.Application.Features.Themes.Queries.GetAllThemes
{
    public class GetAllThemesQueryHandler : IRequestHandler<GetAllThemesQuery, GetAllThemesResponse>
    {
        private readonly IThemeRepository _themeRepository;

        public GetAllThemesQueryHandler(IThemeRepository themeRepository)
        {
            _themeRepository = themeRepository;
        }

        public async Task<GetAllThemesResponse> Handle(GetAllThemesQuery request, CancellationToken cancellationToken)
        {
            var themes = await _themeRepository.GetAllAsync(request.ActiveOnly);

            var themeDtos = themes.Select(t => new ThemeDto
            {
                Id = t.Id,
                Code = t.Code,
                Name = t.Name,
                PreviewImage = t.PreviewImage,
                IsActive = t.IsActive,
                Config = t.Config,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToList();

            return new GetAllThemesResponse
            {
                Success = true,
                Themes = themeDtos
            };
        }
    }
}
