using MediatR;
using SharedKernel.Contracts;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.WebsiteProductFeatures.Queries.GetInventoryProducts
{
    public class GetInventoryProductsQueryHandler : IRequestHandler<GetInventoryProductsQueryRequest, GetInventoryProductsQueryResponse>
    {
        private readonly IInventoryReadService _inventoryReadService;
        private readonly IUnitOfWork _unitOfWork;

        public GetInventoryProductsQueryHandler(
            IInventoryReadService inventoryReadService,
            IUnitOfWork unitOfWork)
        {
            _inventoryReadService = inventoryReadService;
            _unitOfWork = unitOfWork;
        }

        public async Task<GetInventoryProductsQueryResponse> Handle(GetInventoryProductsQueryRequest request, CancellationToken cancellationToken)
        {
            // Get inventory products from Inventory module via SharedKernel interface
            var searchRequest = new InventoryProductSearchRequest
            {
                Page = request.Page,
                PageSize = request.PageSize,
                SortBy = request.SortBy,
                SortDirection = request.SortDirection,
                CategoryId = request.InventoryCategoryId,
                IsActive = request.IsActive,
                PriceFrom = request.PriceFrom,
                PriceTo = request.PriceTo,
                SearchTerm = request.SearchTerm
            };

            var inventoryResult = await _inventoryReadService.SearchProductsAsync(searchRequest);

            // Get already published product IDs
            var websiteProductRepo = _unitOfWork.Repository<WebsiteProduct>();
            var inventoryIds = inventoryResult.Items.Select(p => p.Id).ToList();
            var publishedProducts = await websiteProductRepo.GetAllAsync(
                wp => inventoryIds.Contains(wp.InventoryProductId));

            var publishedMap = publishedProducts.ToDictionary(wp => wp.InventoryProductId);

            // Map to response with IsAlreadyPublished flag
            var items = inventoryResult.Items.Select(p => new InventoryProductListItem
            {
                Id = p.Id,
                Sku = p.Sku,
                Name = p.Name,
                Description = p.Description,
                UnitOfMeasure = p.UnitOfMeasure,
                SalePrice = p.SalePrice,
                CostPrice = p.CostPrice,
                IsActive = p.IsActive,
                ProductBarcode = p.ProductBarcode,
                MainSupplierName = p.MainSupplierName,
                Tax = p.Tax,
                OrderLimit = p.OrderLimit,
                CategoryId = p.CategoryId,
                CategoryName = p.CategoryName,
                MainImageUrl = p.MainImageUrl,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                IsAlreadyPublished = publishedMap.ContainsKey(p.Id),
                WebsiteProductId = publishedMap.ContainsKey(p.Id) ? publishedMap[p.Id].Id : null
            }).ToList();

            return new GetInventoryProductsQueryResponse
            {
                Items = items,
                TotalCount = inventoryResult.TotalCount,
                Page = inventoryResult.Page,
                PageSize = inventoryResult.PageSize
            };
        }
    }
}
