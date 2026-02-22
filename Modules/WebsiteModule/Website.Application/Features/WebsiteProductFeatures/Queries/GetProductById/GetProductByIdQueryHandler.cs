using Website.Application.Contracts.Persistence.Repositories;
using SharedKernel.Contracts;
using SharedKernel.Core.Files;
using MediatR;

namespace Website.Application.Features.WebsiteProductFeatures.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQueryRequest, GetProductByIdQueryResponse>
    {
        private readonly IWebsiteProductRepository _productRepository;
        private readonly IFileUrlResolver _urlResolver;
        private readonly IInventoryReadService _inventoryReadService;

        public GetProductByIdQueryHandler(
            IWebsiteProductRepository productRepository,
            IFileUrlResolver urlResolver,
            IInventoryReadService inventoryReadService)
        {
            _productRepository = productRepository;
            _urlResolver = urlResolver;
            _inventoryReadService = inventoryReadService;
        }

        public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
            // 1. Fetch the Website product (owns images, price, publish state, etc.)
            var product = await _productRepository.GetProductWithImagesAsync(request.Id);

            if (product == null)
                return new GetProductByIdQueryResponse();

            // 2. Fetch the Inventory product read-only via the SharedKernel abstraction
            //    to retrieve product attributes.
            //    The Website module has NO direct dependency on Inventory domain entities.
            var inventoryProduct = await _inventoryReadService
                .GetProductByIdAsync(product.InventoryProductId);

            // 3. Map SharedKernel ProductAttributeDto → Website-local ProductAttributeDto.
            //    Gracefully returns an empty list when the inventory product is unavailable.
            var attributes = inventoryProduct?.Attributes
                .Select(a => new ProductAttributeDto
                {
                    AttributeName  = a.AttributeName,
                    AttributeValue = a.AttributeValue
                })
                .ToList() ?? new List<ProductAttributeDto>();

            // 4. Assemble the final response DTO.
            var dto = new ProductDetailDto
            {
                Id                 = product.Id,
                InventoryProductId = product.InventoryProductId,
                Name               = product.NameSnapshot,
                Images = product.Images
                    .Select(img => new WebsiteProductImageDto
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
                IsPublished  = product.IsPublished,
                DisplayOrder = product.DisplayOrder,
                CreatedAt    = product.CreatedAt,
                UpdatedAt    = product.UpdatedAt,
                Attributes   = attributes
            };

            return new GetProductByIdQueryResponse { Product = dto };
        }
    }
}
