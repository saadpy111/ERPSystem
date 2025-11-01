using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductDtos;
using Inventory.Application.Pagination;
using Inventory.Domain.Entities;
using MediatR;
using System.Linq.Expressions;
using System.Linq;

namespace Inventory.Application.Features.ProductFeatures.Queries.GetPagedProducts
{
    public class GetPagedProductsQueryHandler : IRequestHandler<GetPagedProductsQueryRequest, GetPagedProductsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IproductRepo _productRepo;

        public GetPagedProductsQueryHandler(IUnitOfWork unitOfWork , IproductRepo productRepo)
        {
            _unitOfWork = unitOfWork;
            _productRepo = productRepo;
        }

        public async Task<GetPagedProductsQueryResponse> Handle(GetPagedProductsQueryRequest request, CancellationToken cancellationToken)
        {
            Expression<Func<Product, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(request.Search))
                filter = p => p.Name.Contains(request.Search);

            var pagedResult = await _productRepo
                .SearchProducts(
                    filter,
                    request.Page,
                    request.PageSize,
                    null
                );

            var products = pagedResult.Items.ToList();
    

            var dtoResult = new PagedResult<GetProductDto>
            {
                Items = products.Select(p => p.ToDto()),
                TotalCount = pagedResult.TotalCount
            };

            return new GetPagedProductsQueryResponse
            {
                Result = dtoResult
            };
        }
    }
}