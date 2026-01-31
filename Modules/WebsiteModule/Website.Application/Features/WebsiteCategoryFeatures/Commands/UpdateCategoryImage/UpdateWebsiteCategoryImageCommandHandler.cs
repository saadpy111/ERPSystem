using MediatR;
using Website.Application.Contracts.Infrastruture.FileService;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.WebsiteCategoryFeatures.Commands.UpdateCategoryImage
{
    public class UpdateWebsiteCategoryImageCommandHandler : IRequestHandler<UpdateWebsiteCategoryImageCommandRequest, UpdateWebsiteCategoryImageCommandResponse>
    {
        private readonly IWebsiteCategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public UpdateWebsiteCategoryImageCommandHandler(
            IWebsiteCategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            IFileService fileService)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<UpdateWebsiteCategoryImageCommandResponse> Handle(UpdateWebsiteCategoryImageCommandRequest request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                return new UpdateWebsiteCategoryImageCommandResponse
                {
                    Success = false,
                    Message = "Category not found."
                };
            }

            if (request.DeleteExisting || request.Image != null)
            {
                if (!string.IsNullOrEmpty(category.ImagePath))
                {
                    await _fileService.DeleteFileAsync(category.ImagePath);
                    category.ImagePath = null;
                }
            }

            if (request.Image != null)
            {
                var path = await _fileService.SaveFileAsync(request.Image, "websitecategories");
                category.ImagePath = path;
            }

            category.UpdatedAt = DateTime.UtcNow;
            _categoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateWebsiteCategoryImageCommandResponse
            {
                Success = true,
                Message = "Category image updated successfully.",
                ImagePath = category.ImagePath
            };
        }
    }
}
