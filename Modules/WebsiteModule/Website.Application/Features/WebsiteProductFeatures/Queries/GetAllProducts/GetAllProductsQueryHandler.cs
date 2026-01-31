using MediatR;
using System.Linq.Expressions;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.Pagination;
using SharedKernel.Core.Files;
using Website.Domain.Entities;

namespace Website.Application.Features.WebsiteProductFeatures.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, GetAllProductsQueryResponse>
    {
        private readonly IWebsiteProductRepository _productRepository;
        private readonly IFileUrlResolver _urlResolver;

        public GetAllProductsQueryHandler(IWebsiteProductRepository productRepository, IFileUrlResolver urlResolver)
        {
            _productRepository = productRepository;
            _urlResolver = urlResolver;
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
                includes: new Expression<Func<WebsiteProduct, object>>[] { p => p.Category!, p => p.Images }
            );

            var dtoResult = new PagedResult<ProductListDto>
            {
                Items = result.Items.Select(p => {
                    var primaryImage = p.Images.FirstOrDefault(i => i.IsPrimary) ?? p.Images.OrderBy(i => i.DisplayOrder).FirstOrDefault();
                    return new ProductListDto
                    {
                        Id = p.Id,
                        InventoryProductId = p.InventoryProductId,
                        Name = p.NameSnapshot,
                        ImageUrl = _urlResolver.Resolve(primaryImage?.ImagePath),
                        CategoryId = p.CategoryId,
                        CategoryName = p.CategoryNameSnapshot,
                        Price = p.Price,
                        IsAvailable = p.IsAvailable,
                        IsPublished = p.IsPublished,
                        DisplayOrder = p.DisplayOrder
                    };
                }).ToList(),
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize
            };

            return new GetAllProductsQueryResponse { Result = dtoResult };
        }
    }
}
