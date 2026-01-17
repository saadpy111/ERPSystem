using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.Pagination;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontProducts
{
    public class GetStorefrontProductsQueryHandler : IRequestHandler<GetStorefrontProductsQueryRequest, GetStorefrontProductsQueryResponse>
    {
        private readonly IWebsiteProductRepository _productRepository;

        public GetStorefrontProductsQueryHandler(IWebsiteProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<GetStorefrontProductsQueryResponse> Handle(GetStorefrontProductsQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _productRepository.SearchAsync(
                filter: p =>
                    p.IsPublished && p.IsAvailable &&
                    (request.CategoryId == null || p.CategoryId == request.CategoryId) &&
                    (request.MinPrice == null || p.Price >= request.MinPrice) &&
                    (request.MaxPrice == null || p.Price <= request.MaxPrice) &&
                    (string.IsNullOrEmpty(request.Search) || p.NameSnapshot.Contains(request.Search)),
                page: request.Page,
                pageSize: request.PageSize,
                orderBy: q => q.OrderBy(p => p.DisplayOrder).ThenBy(p => p.NameSnapshot)
            );

            var dtoResult = new PagedResult<StorefrontProductDto>
            {
                Items = result.Items.Select(p => new StorefrontProductDto
                {
                    Id = p.Id,
                    Name = p.NameSnapshot,
                    ImageUrl = p.ImageUrlSnapshot,
                    CategoryName = p.CategoryNameSnapshot,
                    Price = p.Price,
                    IsAvailable = p.IsAvailable
                }).ToList(),
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize
            };

            return new GetStorefrontProductsQueryResponse { Result = dtoResult };
        }
    }
}
