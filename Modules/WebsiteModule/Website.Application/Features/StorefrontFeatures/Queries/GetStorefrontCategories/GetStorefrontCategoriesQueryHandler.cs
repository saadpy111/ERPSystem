using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.Pagination;
using SharedKernel.Core.Files;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontCategories
{
    public class GetStorefrontCategoriesQueryHandler : IRequestHandler<GetStorefrontCategoriesQueryRequest, GetStorefrontCategoriesQueryResponse>
    {
        private readonly IWebsiteCategoryRepository _categoryRepository;
        private readonly IFileUrlResolver _urlResolver;

        public GetStorefrontCategoriesQueryHandler(IWebsiteCategoryRepository categoryRepository, IFileUrlResolver urlResolver)
        {
            _categoryRepository = categoryRepository;
            _urlResolver = urlResolver;
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
                ImageUrl = _urlResolver.Resolve(c.ImagePath),
                ProductCount = c.Products?.Count(p => p.IsPublished && p.IsAvailable) ?? 0
            }).ToList();

            // Build tree
            var allRoots = dtos.Where(c => c.ParentCategoryId == null).ToList();

            // Filter by search term if provided
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                allRoots = allRoots
                    .Where(r => r.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            foreach (var root in allRoots)
            {
                BuildTree(root, dtos);
            }

            // Paginate Roots
            var totalCount = allRoots.Count;
            var pagedRoots = allRoots
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new GetStorefrontCategoriesQueryResponse
            {
                Result = new PagedResult<StorefrontCategoryDto>
                {
                    Items = pagedRoots,
                    TotalCount = totalCount,
                    Page = request.PageNumber,
                    PageSize = request.PageSize
                }
            };
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
