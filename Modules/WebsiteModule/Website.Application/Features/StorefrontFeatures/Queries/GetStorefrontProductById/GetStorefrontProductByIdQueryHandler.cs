using MediatR;
using SharedKernel.Contracts;
using SharedKernel.Core.Files;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontProductById
{
    public class GetStorefrontProductByIdQueryHandler
        : IRequestHandler<GetStorefrontProductByIdQueryRequest, GetStorefrontProductByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;
        private readonly IFileUrlResolver _urlResolver;
        private readonly IInventoryReadService _inventoryReadService;

        public GetStorefrontProductByIdQueryHandler(
            IUnitOfWork unitOfWork,
            ITenantProvider tenantProvider,
            IFileUrlResolver urlResolver,
            IInventoryReadService inventoryReadService)
        {
            _unitOfWork          = unitOfWork;
            _tenantProvider      = tenantProvider;
            _urlResolver         = urlResolver;
            _inventoryReadService = inventoryReadService;
        }

        public async Task<GetStorefrontProductByIdQueryResponse> Handle(
            GetStorefrontProductByIdQueryRequest request,
            CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<WebsiteProduct>();

            // 1. Fetch published WebsiteProduct with images (existing query — unchanged).
            var product = await repo.GetFirstAsync(
                p => p.Id == request.Id && p.IsPublished,
                asNoTracking: true,
                p => p.Images);

            if (product == null)
                return new GetStorefrontProductByIdQueryResponse { Product = null };

            // 2. Fetch Inventory product read-only via the SharedKernel abstraction.
            //    IInventoryReadService.GetProductByIdAsync already eager-loads
            //    AttributeValues → Attribute inside the Inventory module.
            //    The Website module has zero dependency on Inventory domain entities.
            var inventoryProduct = await _inventoryReadService
                .GetProductByIdAsync(product.InventoryProductId);

            // 3. Map SharedKernel ProductAttributeDto → StorefrontProductAttributeDto.
            //    Gracefully returns an empty list when the inventory product is unavailable.
            var attributes = inventoryProduct?.Attributes
                .Select(a => new StorefrontProductAttributeDto
                {
                    AttributeName  = a.AttributeName,
                    AttributeValue = a.AttributeValue
                })
                .ToList() ?? new List<StorefrontProductAttributeDto>();

            // 4. Build response — image mapping is identical to original, attributes added.
            return new GetStorefrontProductByIdQueryResponse
            {
                Product = new StorefrontProductDetailDto
                {
                    Id   = product.Id,
                    Name = product.NameSnapshot,
                    Images = product.Images
                        .Select(img => new StorefrontProductImageDto
                        {
                            Id           = img.Id,
                            ImageUrl     = _urlResolver.Resolve(img.ImagePath) ?? string.Empty,
                            AltText      = img.AltText,
                            IsPrimary    = img.IsPrimary,
                            DisplayOrder = img.DisplayOrder
                        })
                        .OrderBy(i => i.DisplayOrder)
                        .ToList(),
                    CategoryId   = product.CategoryId,
                    CategoryName = product.CategoryNameSnapshot,
                    Price        = product.Price,
                    IsAvailable  = product.IsAvailable,
                    Attributes   = attributes
                }
            };
        }
    }
}
