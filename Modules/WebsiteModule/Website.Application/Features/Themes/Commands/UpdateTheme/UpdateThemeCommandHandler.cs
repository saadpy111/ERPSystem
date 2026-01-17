using MediatR;
using Website.Application.Contracts.Persistence;

namespace Website.Application.Features.Themes.Commands.UpdateTheme
{
    public class UpdateThemeCommandHandler : IRequestHandler<UpdateThemeCommand, UpdateThemeResponse>
    {
        private readonly IThemeRepository _themeRepository;
        private readonly IWebsiteUnitOfWork _unitOfWork;

        public UpdateThemeCommandHandler(IThemeRepository themeRepository, IWebsiteUnitOfWork unitOfWork)
        {
            _themeRepository = themeRepository;
            _unitOfWork = unitOfWork;
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

            // Update theme properties (code cannot be changed)
            theme.Name = request.Name;
            theme.PreviewImage = request.PreviewImage;
            theme.IsActive = request.IsActive;
            theme.Config = request.Config;

            await _themeRepository.UpdateAsync(theme);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateThemeResponse { Success = true };
        }
    }
}
