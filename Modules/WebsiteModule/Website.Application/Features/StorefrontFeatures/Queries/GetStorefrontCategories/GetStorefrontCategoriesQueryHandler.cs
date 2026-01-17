using MediatR;
using Website.Application.Contracts.Persistence.Repositories;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontCategories
{
    public class GetStorefrontCategoriesQueryHandler : IRequestHandler<GetStorefrontCategoriesQueryRequest, GetStorefrontCategoriesQueryResponse>
    {
        private readonly IWebsiteCategoryRepository _categoryRepository;

        public GetStorefrontCategoriesQueryHandler(IWebsiteCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<GetStorefrontCategoriesQueryResponse> Handle(GetStorefrontCategoriesQueryRequest request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetActiveCategoriesAsync();

            var dtos = categories.Select(c => new StorefrontCategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                ParentCategoryId = c.ParentCategoryId,
                ProductCount = c.Products?.Count(p => p.IsPublished && p.IsAvailable) ?? 0
            }).ToList();

            // Build tree
            var roots = dtos.Where(c => c.ParentCategoryId == null).ToList();
            foreach (var root in roots)
            {
                BuildTree(root, dtos);
            }

            return new GetStorefrontCategoriesQueryResponse { Categories = roots };
        }

        private void BuildTree(StorefrontCategoryDto parent, List<StorefrontCategoryDto> all)
        {
            parent.Children = all.Where(c => c.ParentCategoryId == parent.Id).ToList();
            foreach (var child in parent.Children)
            {
                BuildTree(child, all);
            }
        }
    }
}
