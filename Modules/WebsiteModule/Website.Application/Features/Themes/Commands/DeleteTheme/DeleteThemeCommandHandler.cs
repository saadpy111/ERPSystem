using MediatR;
using Website.Application.Contracts.Persistence;
using Website.Application.Contracts.Infrastruture.FileService;

namespace Website.Application.Features.Themes.Commands.DeleteTheme
{
    public class DeleteThemeCommandHandler : IRequestHandler<DeleteThemeCommand, DeleteThemeResponse>
    {
        private readonly IThemeRepository _themeRepository;
        private readonly IWebsiteUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public DeleteThemeCommandHandler(
            IThemeRepository themeRepository, 
            IWebsiteUnitOfWork unitOfWork,
            IFileService fileService)
        {
            _themeRepository = themeRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<DeleteThemeResponse> Handle(DeleteThemeCommand request, CancellationToken cancellationToken)
        {
            var theme = await _themeRepository.GetByIdAsync(request.ThemeId);

            if (theme == null)
            {
                return new DeleteThemeResponse
                {
                    Success = false,
                    Error = "Theme not found"
                };
            }

            // Delete Associated Images
            if (!string.IsNullOrEmpty(theme.PreviewImage))
            {
                await _fileService.DeleteFileAsync(theme.PreviewImage);
            }

            if (!string.IsNullOrEmpty(theme.Config.Hero.BackgroundImage))
            {
                await _fileService.DeleteFileAsync(theme.Config.Hero.BackgroundImage);
            }

            // Delete Record
            await _themeRepository.DeleteAsync(request.ThemeId);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new DeleteThemeResponse { Success = true };
        }
    }
}
