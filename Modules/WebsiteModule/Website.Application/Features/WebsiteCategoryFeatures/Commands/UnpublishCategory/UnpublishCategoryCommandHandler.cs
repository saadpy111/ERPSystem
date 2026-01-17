using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.WebsiteCategoryFeatures.Commands.UnpublishCategory
{
    public class UnpublishCategoryCommandHandler : IRequestHandler<UnpublishCategoryCommandRequest, UnpublishCategoryCommandResponse>
    {
        private readonly IWebsiteCategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UnpublishCategoryCommandHandler(
            IWebsiteCategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UnpublishCategoryCommandResponse> Handle(UnpublishCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetCategoryWithChildrenAsync(request.WebsiteCategoryId);
            if (category == null)
            {
                return new UnpublishCategoryCommandResponse
                {
                    Success = false,
                    Message = "Website category not found."
                };
            }

            // Idempotent: if already inactive, just return success
            if (!category.IsActive)
            {
                return new UnpublishCategoryCommandResponse
                {
                    Success = true,
                    Message = "Category is already unpublished."
                };
            }

            // Set IsActive = false for category
            category.IsActive = false;
            category.UpdatedAt = DateTime.UtcNow;
            _categoryRepository.Update(category);

            // Also deactivate all child categories recursively
            var childCategoryIds = await GetAllChildCategoryIds(request.WebsiteCategoryId);
            var childCategories = await _categoryRepository.GetAllAsync(c => childCategoryIds.Contains(c.Id));
            foreach (var child in childCategories)
            {
                child.IsActive = false;
                child.UpdatedAt = DateTime.UtcNow;
                _categoryRepository.Update(child);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Note: Products under inactive categories will be hidden from storefront 
            // by filtering queries (products remain published but category is inactive)

            return new UnpublishCategoryCommandResponse
            {
                Success = true,
                ProductsAffected = 0 // Products are hidden via category filter, not unpublished
            };
        }

        private async Task<List<Guid>> GetAllChildCategoryIds(Guid parentId)
        {
            var result = new List<Guid>();
            var children = await _categoryRepository.GetAllAsync(c => c.ParentCategoryId == parentId);
            
            foreach (var child in children)
            {
                result.Add(child.Id);
                var grandChildren = await GetAllChildCategoryIds(child.Id);
                result.AddRange(grandChildren);
            }
            
            return result;
        }
    }
}
