using MediatR;
using Website.Application.Contracts.Persistence;
using Website.Domain.Entities;
using SharedKernel.Website;
using Website.Domain.ValueObjects;

namespace Website.Application.Features.Themes.Commands.CreateTheme
{
    public class CreateThemeCommandHandler : IRequestHandler<CreateThemeCommand, CreateThemeResponse>
    {
        private readonly IThemeRepository _themeRepository;
        private readonly IWebsiteUnitOfWork _unitOfWork;
        private readonly IWebsiteImageService _websiteImageService;

        public CreateThemeCommandHandler(
            IThemeRepository themeRepository, 
            IWebsiteUnitOfWork unitOfWork,
            IWebsiteImageService websiteImageService)
        {
            _themeRepository = themeRepository;
            _unitOfWork = unitOfWork;
            _websiteImageService = websiteImageService;
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

            // Process Images
            string previewImagePath = string.Empty;
            string heroBackgroundImagePath = string.Empty;

            if (request.PreviewImageFile != null)
            {
                previewImagePath = await _websiteImageService.ProcessThemePreviewImageAsync(request.Code, request.PreviewImageFile);
            }

            if (request.HeroBackgroundImageFile != null)
            {
                heroBackgroundImagePath = await _websiteImageService.ProcessThemeHeroImageAsync(request.Code, request.HeroBackgroundImageFile);
            }

            var theme = new Theme
            {
                Id = Guid.NewGuid(),
                Code = request.Code,
                Name = request.Name,
                PreviewImage = previewImagePath,
                IsActive = request.IsActive,
                Config = new ThemeConfig
                {
                    Colors = new ThemeColors
                    {
                        Primary = request.PrimaryColor ?? string.Empty,
                        Secondary = request.SecondaryColor ?? string.Empty,
                        Background = request.BackgroundColor ?? string.Empty,
                        Text = request.TextColor ?? string.Empty
                    },
                    Hero = new HeroSection
                    {
                        Title = request.HeroTitle ?? string.Empty,
                        Subtitle = request.HeroSubtitle ?? string.Empty,
                        ButtonText = request.HeroButtonText ?? string.Empty,
                        BackgroundImage = heroBackgroundImagePath
                    },
                    Sections = request.Sections ?? new List<SectionItem>()
                }
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
