using MediatR;
using Website.Application.Contracts.Persistence.Repositories;

namespace Website.Application.Features.WebsiteProductFeatures.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQueryRequest, GetProductByIdQueryResponse>
    {
        private readonly IWebsiteProductRepository _productRepository;

        public GetProductByIdQueryHandler(IWebsiteProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductWithCategoryAsync(request.Id);

            if (product == null)
            {
                return new GetProductByIdQueryResponse();
            }

            var dto = new ProductDetailDto
            {
                Id = product.Id,
                InventoryProductId = product.InventoryProductId,
                Name = product.NameSnapshot,
                ImageUrl = product.ImageUrlSnapshot,
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
