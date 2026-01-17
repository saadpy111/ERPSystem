using MediatR;
using Website.Application.Contracts.Persistence;

namespace Website.Application.Features.Themes.Commands.DeleteTheme
{
    public class DeleteThemeCommandHandler : IRequestHandler<DeleteThemeCommand, DeleteThemeResponse>
    {
        private readonly IThemeRepository _themeRepository;
        private readonly IWebsiteUnitOfWork _unitOfWork;

        public DeleteThemeCommandHandler(IThemeRepository themeRepository, IWebsiteUnitOfWork unitOfWork)
        {
            _themeRepository = themeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteThemeResponse> Handle(DeleteThemeCommand request, CancellationToken cancellationToken)
        {
            var deleted = await _themeRepository.DeleteAsync(request.ThemeId);

            if (!deleted)
            {
                return new DeleteThemeResponse
                {
                    Success = false,
                    Error = "Theme not found"
                };
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new DeleteThemeResponse { Success = true };
        }
    }
}
