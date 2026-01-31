using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using SharedKernel.Core.Files;

namespace Website.Application.Features.WebsiteCategoryFeatures.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQueryRequest, GetAllCategoriesQueryResponse>
    {
        private readonly IWebsiteCategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileUrlResolver _urlResolver;

        public GetAllCategoriesQueryHandler(
            IWebsiteCategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            IFileUrlResolver urlResolver)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _urlResolver = urlResolver;
        }

        public async Task<GetAllCategoriesQueryResponse> Handle(GetAllCategoriesQueryRequest request, CancellationToken cancellationToken)
        {
            var categories = request.IsActive == true
                ? await _categoryRepository.GetActiveCategoriesAsync()
                : await _categoryRepository.GetAllAsync(
                    filter: request.IsActive.HasValue ? c => c.IsActive == request.IsActive : null);

            var dtoList = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                InventoryCategoryId = c.InventoryCategoryId,
                Name = c.Name,
                Slug = c.Slug,
                ParentCategoryId = c.ParentCategoryId,
                ImageUrl = _urlResolver.Resolve(c.ImagePath),
                DisplayOrder = c.DisplayOrder,
                IsActive = c.IsActive,
                ProductCount = c.Products?.Count ?? 0
            }).ToList();

            if (request.IncludeTree)
            {
                var rootCategories = dtoList.Where(c => c.ParentCategoryId == null).ToList();
                foreach (var root in rootCategories)
                {
                    BuildTree(root, dtoList);
                }
                return new GetAllCategoriesQueryResponse { Categories = rootCategories };
            }

            return new GetAllCategoriesQueryResponse { Categories = dtoList };
        }

        private void BuildTree(CategoryDto parent, List<CategoryDto> all)
        {
            parent.Children = all.Where(c => c.ParentCategoryId == parent.Id).ToList();
            foreach (var child in parent.Children)
            {
                BuildTree(child, all);
            }
        }
    }
}
