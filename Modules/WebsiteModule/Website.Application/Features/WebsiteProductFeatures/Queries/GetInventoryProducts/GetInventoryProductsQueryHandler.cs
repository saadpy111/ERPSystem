using MediatR;
using SharedKernel.Contracts;
using SharedKernel.Core.Files;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.WebsiteProductFeatures.Queries.GetInventoryProducts
{
    public class GetInventoryProductsQueryHandler
        : IRequestHandler<GetInventoryProductsQueryRequest, GetInventoryProductsQueryResponse>
    {
        private readonly IInventoryReadService _inventoryReadService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileUrlResolver _fileUrlResolver;

        public GetInventoryProductsQueryHandler(
            IInventoryReadService inventoryReadService,
            IUnitOfWork unitOfWork,
            IFileUrlResolver fileUrlResolver)
        {
            _inventoryReadService = inventoryReadService;
            _unitOfWork = unitOfWork;
            _fileUrlResolver = fileUrlResolver;
        }

        public async Task<GetInventoryProductsQueryResponse> Handle(
            GetInventoryProductsQueryRequest request,
            CancellationToken cancellationToken)
        {
            // ===== 1. Get inventory products =====
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

            var inventoryResult =
                await _inventoryReadService.SearchProductsAsync(searchRequest);

            // ===== 2. Get published website products =====
            var websiteProductRepo = _unitOfWork.Repository<WebsiteProduct>();

            var inventoryIds = inventoryResult.Items.Select(p => p.Id).ToList();

            var publishedProducts = await websiteProductRepo.GetAllAsync(
                wp => inventoryIds.Contains(wp.InventoryProductId)
         );

            var publishedMap =
                publishedProducts.ToDictionary(wp => wp.InventoryProductId);

            // ===== 3. Map =====
            var items = inventoryResult.Items.Select(p =>
            {
                publishedMap.TryGetValue(p.Id, out var websiteProduct);

                var images = p.Images?
                    .OrderBy(i => i.DisplayOrder)
                    .Select(i => new InventoryProductImageDto
                    {
                        Id = i.Id,
                        ImageUrl = _fileUrlResolver.Resolve(i.ImageUrl)!,
                        IsPrimary = i.IsPrimary,
                        DisplayOrder = i.DisplayOrder
                    }).ToList() ?? new();

                var mainImage =
                    images.FirstOrDefault(i => i.IsPrimary)?.ImageUrl
                    ?? images.FirstOrDefault()?.ImageUrl;

                return new InventoryProductListItem
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

                    Images = images,
                    MainImageUrl = mainImage,

                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,

                    IsAlreadyPublished = websiteProduct != null,
                    WebsiteProductId = websiteProduct?.Id
                };
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
