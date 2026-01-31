using MediatR;
using Website.Application.Contracts.Persistence;
using SharedKernel.Website;
using Website.Application.Contracts.Infrastruture.FileService;
using Website.Domain.ValueObjects;

namespace Website.Application.Features.Themes.Commands.UpdateTheme
{
    public class UpdateThemeCommandHandler : IRequestHandler<UpdateThemeCommand, UpdateThemeResponse>
    {
        private readonly IThemeRepository _themeRepository;
        private readonly IWebsiteUnitOfWork _unitOfWork;
        private readonly IWebsiteImageService _websiteImageService;
        private readonly IFileService _fileService;

        public UpdateThemeCommandHandler(
            IThemeRepository themeRepository, 
            IWebsiteUnitOfWork unitOfWork,
            IWebsiteImageService websiteImageService,
            IFileService fileService)
        {
            _themeRepository = themeRepository;
            _unitOfWork = unitOfWork;
            _websiteImageService = websiteImageService;
            _fileService = fileService;
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
            theme.IsActive = request.IsActive;

            // Handle Preview Image Replacement
            if (request.PreviewImageFile != null)
            {
                // Delete old image
                if (!string.IsNullOrEmpty(theme.PreviewImage))
                {
                    await _fileService.DeleteFileAsync(theme.PreviewImage);
                }

                // Save new image
                theme.PreviewImage = await _websiteImageService.ProcessThemePreviewImageAsync(theme.Code, request.PreviewImageFile);
            }

            // Handle Hero Background Image Replacement
            if (request.HeroBackgroundImageFile != null)
            {
                // Delete old image
                if (!string.IsNullOrEmpty(theme.Config.Hero.BackgroundImage))
                {
                    await _fileService.DeleteFileAsync(theme.Config.Hero.BackgroundImage);
                }

                // Save new image
                theme.Config.Hero.BackgroundImage = await _websiteImageService.ProcessThemeHeroImageAsync(theme.Code, request.HeroBackgroundImageFile);
            }

            // Map flattened Colors
            if (request.PrimaryColor != null)
            {
                theme.Config.Colors.Primary = request.PrimaryColor;
                theme.Config.Colors.Secondary = request.SecondaryColor ?? theme.Config.Colors.Secondary;
                theme.Config.Colors.Background = request.BackgroundColor ?? theme.Config.Colors.Background;
                theme.Config.Colors.Text = request.TextColor ?? theme.Config.Colors.Text;
            }

            // Map flattened Hero Text
            if (request.HeroTitle != null)
            {
                theme.Config.Hero.Title = request.HeroTitle;
                theme.Config.Hero.Subtitle = request.HeroSubtitle ?? theme.Config.Hero.Subtitle;
                theme.Config.Hero.ButtonText = request.HeroButtonText ?? theme.Config.Hero.ButtonText;
            }

            if (request.Sections != null)
            {
                theme.Config.Sections = request.Sections;
            }

            await _themeRepository.UpdateAsync(theme);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateThemeResponse { Success = true };
        }
    }
}
