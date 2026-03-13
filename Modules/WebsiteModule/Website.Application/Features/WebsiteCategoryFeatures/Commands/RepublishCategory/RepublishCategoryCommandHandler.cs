using MediatR;
using SharedKernel.Contracts;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.Contracts.Infrastruture.FileService;

namespace Website.Application.Features.WebsiteCategoryFeatures.Commands.RepublishCategory
{
    public class RepublishCategoryCommandHandler : IRequestHandler<RepublishCategoryCommandRequest, RepublishCategoryCommandResponse>
    {
        private readonly IWebsiteCategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInventoryReadService _inventoryReadService;
        private readonly IFileService _fileService;

        public RepublishCategoryCommandHandler(
            IWebsiteCategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            IInventoryReadService inventoryReadService,
            IFileService fileService)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _inventoryReadService = inventoryReadService;
            _fileService = fileService;
        }

        public async Task<RepublishCategoryCommandResponse> Handle(RepublishCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.WebsiteCategoryId);
            if (category == null)
            {
                return new RepublishCategoryCommandResponse
                {
                    Success = false,
                    Message = "Website category not found."
                };
            }


            category.IsActive = true;
            category.UpdatedAt = DateTime.UtcNow;

            _categoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RepublishCategoryCommandResponse
            {
                Success = true,
                Message = "Category republished successfully."
            };
        }
    }
}
