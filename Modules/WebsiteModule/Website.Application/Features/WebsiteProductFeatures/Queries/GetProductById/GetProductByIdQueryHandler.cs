using Website.Application.Contracts.Persistence.Repositories;
using SharedKernel.Core.Files;
using MediatR;

namespace Website.Application.Features.WebsiteProductFeatures.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQueryRequest, GetProductByIdQueryResponse>
    {
        private readonly IWebsiteProductRepository _productRepository;
        private readonly IFileUrlResolver _urlResolver;

        public GetProductByIdQueryHandler(IWebsiteProductRepository productRepository, IFileUrlResolver urlResolver)
        {
            _productRepository = productRepository;
            _urlResolver = urlResolver;
        }

        public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductWithImagesAsync(request.Id);

            if (product == null)
            {
                return new GetProductByIdQueryResponse();
            }

            var dto = new ProductDetailDto
            {
                Id = product.Id,
                InventoryProductId = product.InventoryProductId,
                Name = product.NameSnapshot,
                Images = product.Images.Select(img => new WebsiteProductImageDto
                {
                    Id = img.Id,
                    ImageUrl = _urlResolver.Resolve(img.ImagePath) ?? string.Empty,
                    AltText = img.AltText,
                    IsPrimary = img.IsPrimary,
                    DisplayOrder = img.DisplayOrder
                }).OrderBy(i => i.DisplayOrder).ToList(),
                CategoryId = product.CategoryId,
                CategoryName = product.CategoryNameSnapshot,
                Price = product.Price,
                IsAvailable = product.IsAvailable,
                IsPublished = product.IsPublished,
                DisplayOrder = product.DisplayOrder,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };

            return new GetProductByIdQueryResponse { Product = dto };
        }
    }
}
