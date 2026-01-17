using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.Pagination;

namespace Website.Application.Features.WebsiteProductFeatures.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, GetAllProductsQueryResponse>
    {
        private readonly IWebsiteProductRepository _productRepository;

        public GetAllProductsQueryHandler(IWebsiteProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<GetAllProductsQueryResponse> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _productRepository.SearchAsync(
                filter: p =>
                    (request.IsPublished == null || p.IsPublished == request.IsPublished) &&
                    (request.CategoryId == null || p.CategoryId == request.CategoryId) &&
                    (request.MinPrice == null || p.Price >= request.MinPrice) &&
                    (request.MaxPrice == null || p.Price <= request.MaxPrice) &&
                    (string.IsNullOrEmpty(request.Search) || p.NameSnapshot.Contains(request.Search)),
                page: request.Page,
                pageSize: request.PageSize,
                orderBy: q => q.OrderBy(p => p.DisplayOrder).ThenBy(p => p.NameSnapshot),
                includes: p => p.Category!
            );

            var dtoResult = new PagedResult<ProductListDto>
            {
                Items = result.Items.Select(p => new ProductListDto
                {
                    Id = p.Id,
                    InventoryProductId = p.InventoryProductId,
                    Name = p.NameSnapshot,
                    ImageUrl = p.ImageUrlSnapshot,
                    CategoryId = p.CategoryId,
                    CategoryName = p.CategoryNameSnapshot,
                    Price = p.Price,
                    IsAvailable = p.IsAvailable,
                    IsPublished = p.IsPublished,
                    DisplayOrder = p.DisplayOrder
                }).ToList(),
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize
            };

            return new GetAllProductsQueryResponse { Result = dtoResult };
        }
    }
}
