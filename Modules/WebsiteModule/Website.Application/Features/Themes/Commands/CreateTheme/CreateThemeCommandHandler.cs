using MediatR;
using Website.Application.Contracts.Persistence;
using Website.Domain.Entities;

namespace Website.Application.Features.Themes.Commands.CreateTheme
{
    public class CreateThemeCommandHandler : IRequestHandler<CreateThemeCommand, CreateThemeResponse>
    {
        private readonly IThemeRepository _themeRepository;
        private readonly IWebsiteUnitOfWork _unitOfWork;

        public CreateThemeCommandHandler(IThemeRepository themeRepository, IWebsiteUnitOfWork unitOfWork)
        {
            _themeRepository = themeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateThemeResponse> Handle(CreateThemeCommand request, CancellationToken cancellationToken)
        {
            // Validate unique code
            if (await _themeRepository.ExistsAsync(request.Code))
            {
                return new CreateThemeResponse
                {
                    Success = false,
                    Error = $"Theme with code '{request.Code}' already exists"
                };
            }

            var theme = new Theme
            {
                Id = Guid.NewGuid(),
                Code = request.Code,
                Name = request.Name,
                PreviewImage = request.PreviewImage,
                IsActive = request.IsActive,
                Config = request.Config
            };

            await _themeRepository.CreateAsync(theme);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateThemeResponse
            {
                Success = true,
                ThemeId = theme.Id
            };
        }
    }
}
