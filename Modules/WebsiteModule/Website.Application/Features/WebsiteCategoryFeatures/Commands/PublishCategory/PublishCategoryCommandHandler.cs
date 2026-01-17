using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.WebsiteCategoryFeatures.Commands.PublishCategory
{
    public class PublishCategoryCommandHandler : IRequestHandler<PublishCategoryCommandRequest, PublishCategoryCommandResponse>
    {
        private readonly IWebsiteCategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public PublishCategoryCommandHandler(
            IWebsiteCategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            ITenantProvider tenantProvider)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
        }

        public async Task<PublishCategoryCommandResponse> Handle(PublishCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            // Check if already published by inventory ID
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
            }

            var category = new WebsiteCategory
            {
                InventoryCategoryId = request.InventoryCategoryId,
                Name = request.Name,
                Slug = request.Slug ?? GenerateSlug(request.Name),
                ParentCategoryId = request.ParentCategoryId,
                DisplayOrder = request.DisplayOrder,
                IsActive = true,
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
