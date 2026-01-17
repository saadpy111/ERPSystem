using MediatR;
using SharedKernel.Contracts;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.WebsiteCategoryFeatures.Queries.GetInventoryCategories
{
    public class GetInventoryCategoriesQueryHandler : IRequestHandler<GetInventoryCategoriesQueryRequest, GetInventoryCategoriesQueryResponse>
    {
        private readonly IInventoryReadService _inventoryReadService;
        private readonly IUnitOfWork _unitOfWork;

        public GetInventoryCategoriesQueryHandler(
            IInventoryReadService inventoryReadService,
            IUnitOfWork unitOfWork)
        {
            _inventoryReadService = inventoryReadService;
            _unitOfWork = unitOfWork;
        }

        public async Task<GetInventoryCategoriesQueryResponse> Handle(GetInventoryCategoriesQueryRequest request, CancellationToken cancellationToken)
        {
            // Get inventory categories from Inventory module via SharedKernel interface
            var searchRequest = new InventoryCategorySearchRequest
            {
                Page = request.Page,
                PageSize = request.PageSize,
                SortBy = request.SortBy,
                SortDirection = request.SortDirection,
                ParentCategoryId = request.ParentCategoryId,
                SearchTerm = request.SearchTerm
            };

            var inventoryResult = await _inventoryReadService.SearchCategoriesAsync(searchRequest);

            // Get already published category IDs
            var websiteCategoryRepo = _unitOfWork.Repository<WebsiteCategory>();
            var inventoryIds = inventoryResult.Items.Select(c => c.Id).ToList();
            var publishedCategories = await websiteCategoryRepo.GetAllAsync(
                wc => wc.InventoryCategoryId.HasValue && inventoryIds.Contains(wc.InventoryCategoryId.Value));

            var publishedMap = publishedCategories
                .Where(wc => wc.InventoryCategoryId.HasValue)
                .ToDictionary(wc => wc.InventoryCategoryId!.Value);

            // Map to response with IsAlreadyPublished flag
            var items = inventoryResult.Items.Select(c => new InventoryCategoryListItem
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ParentCategoryId = c.ParentCategoryId,
                ParentCategoryName = c.ParentCategoryName,
                IsActive = c.IsActive,
                ProductCount = c.ProductCount,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                IsAlreadyPublished = publishedMap.ContainsKey(c.Id),
                WebsiteCategoryId = publishedMap.ContainsKey(c.Id) ? publishedMap[c.Id].Id : null
            }).ToList();

            return new GetInventoryCategoriesQueryResponse
            {
                Items = items,
                TotalCount = inventoryResult.TotalCount,
                Page = inventoryResult.Page,
                PageSize = inventoryResult.PageSize
            };
        }
    }
}
