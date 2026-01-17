using MediatR;
using Website.Application.Contracts.Persistence;
using Website.Application.DTOs;

namespace Website.Application.Features.Themes.Queries.GetThemeById
{
    public class GetThemeByIdQueryHandler : IRequestHandler<GetThemeByIdQuery, GetThemeByIdResponse>
    {
        private readonly IThemeRepository _themeRepository;

        public GetThemeByIdQueryHandler(IThemeRepository themeRepository)
        {
            _themeRepository = themeRepository;
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
                    PreviewImage = theme.PreviewImage,
                    IsActive = theme.IsActive,
                    Config = theme.Config,
                    CreatedAt = theme.CreatedAt,
                    UpdatedAt = theme.UpdatedAt
                }
            };
        }
    }
}
