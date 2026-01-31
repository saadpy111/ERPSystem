using MediatR;
using SharedKernel.Contracts;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.Contracts.Infrastruture.FileService;
using Website.Domain.Entities;

namespace Website.Application.Features.WebsiteCategoryFeatures.Commands.PublishCategory
{
    public class PublishCategoryCommandHandler : IRequestHandler<PublishCategoryCommandRequest, PublishCategoryCommandResponse>
    {
        private readonly IWebsiteCategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;
        private readonly IInventoryReadService _inventoryReadService;
        private readonly IFileService _fileService;

        public PublishCategoryCommandHandler(
            IWebsiteCategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            ITenantProvider tenantProvider,
            IInventoryReadService inventoryReadService,
            IFileService fileService)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
            _inventoryReadService = inventoryReadService;
            _fileService = fileService;
        }

        public async Task<PublishCategoryCommandResponse> Handle(PublishCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            string? imagePath = null;
            if (request.InventoryCategoryId.HasValue)
            {
                var existing = await _categoryRepository.GetByInventoryCategoryIdAsync(request.InventoryCategoryId.Value);
                if (existing != null)
                {
                    return new PublishCategoryCommandResponse
                    {
                        Success = false,
                        Message = "Category is already published."
                    };
                }

                var inventoryCategory = await _inventoryReadService.GetCategoryByIdAsync(request.InventoryCategoryId.Value);
                if (inventoryCategory != null && !string.IsNullOrEmpty(inventoryCategory.ImagePath))
                {
                    try
                    {
                        imagePath = await _fileService.CopyFileAsync(inventoryCategory.ImagePath, "websitecategories");
                    }
                    catch (Exception)
                    {
                        // Skip if copy fails
                    }
                }
            }

            var category = new WebsiteCategory
            {
                InventoryCategoryId = request.InventoryCategoryId,
                Name = request.Name,
                Slug = request.Slug ?? GenerateSlug(request.Name),
                ParentCategoryId = request.ParentCategoryId,
                DisplayOrder = request.DisplayOrder,
                IsActive = true,
                ImagePath = imagePath,
                TenantId = _tenantProvider.GetTenantId() ?? string.Empty
            };

            await _categoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new PublishCategoryCommandResponse
            {
                Success = true,
                CategoryId = category.Id
            };
        }

        private static string GenerateSlug(string name)
        {
            return name.ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("&", "and");
        }
    }
}
