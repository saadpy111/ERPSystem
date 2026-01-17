using MediatR;
using SharedKernel.Contracts;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.WebsiteProductFeatures.Commands.PublishProduct
{
    public class PublishProductCommandHandler : IRequestHandler<PublishProductCommandRequest, PublishProductCommandResponse>
    {
        private readonly IInventoryReadService _inventoryReadService;
        private readonly IWebsiteProductRepository _productRepository;
        private readonly IWebsiteCategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public PublishProductCommandHandler(
            IInventoryReadService inventoryReadService,
            IWebsiteProductRepository productRepository,
            IWebsiteCategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            ITenantProvider tenantProvider)
        {
            _inventoryReadService = inventoryReadService;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
        }

        public async Task<PublishProductCommandResponse> Handle(PublishProductCommandRequest request, CancellationToken cancellationToken)
        {
            // Fetch product data from Inventory module
            var inventoryProduct = await _inventoryReadService.GetProductByIdAsync(request.InventoryProductId);
            if (inventoryProduct == null)
            {
                return new PublishProductCommandResponse
                {
                    Success = false,
                    Message = "Inventory product not found."
                };
            }

            // Validate Website category
            var category = await _categoryRepository.GetByIdAsync(request.WebsiteCategoryId);
            if (category == null)
            {
                return new PublishProductCommandResponse
                {
                    Success = false,
                    Message = "Website category not found. Publish the category first."
                };
            }

            // Check if already published
            var existing = await _productRepository.GetByInventoryProductIdAsync(request.InventoryProductId);
            if (existing != null)
            {
                // Re-publish: update snapshot from Inventory and set IsPublished = true
                existing.NameSnapshot = inventoryProduct.Name;
                existing.ImageUrlSnapshot = inventoryProduct.MainImageUrl;
                existing.CategoryId = request.WebsiteCategoryId;
                existing.CategoryNameSnapshot = category.Name;
                existing.Price = inventoryProduct.SalePrice;
                existing.IsAvailable = inventoryProduct.IsActive;
                existing.IsPublished = true;
                existing.DisplayOrder = request.DisplayOrder;
                existing.UpdatedAt = DateTime.UtcNow;

                _productRepository.Update(existing);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new PublishProductCommandResponse
                {
                    Success = true,
                    ProductId = existing.Id,
                    Message = "Product re-published with updated snapshot."
                };
            }

            // Create new WebsiteProduct from Inventory data
            var product = new WebsiteProduct
            {
                InventoryProductId = request.InventoryProductId,
                NameSnapshot = inventoryProduct.Name,
                ImageUrlSnapshot = inventoryProduct.MainImageUrl,
                CategoryId = request.WebsiteCategoryId,
                CategoryNameSnapshot = category.Name,
                Price = inventoryProduct.SalePrice,
                IsAvailable = inventoryProduct.IsActive,
                IsPublished = true,
                DisplayOrder = request.DisplayOrder,
                TenantId = _tenantProvider.GetTenantId() ?? string.Empty
            };

            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new PublishProductCommandResponse
            {
                Success = true,
                ProductId = product.Id
            };
        }
    }
}
