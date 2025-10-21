using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductDtos;
using Inventory.Application.Pagination;
using Inventory.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Inventory.Application.Features.ProductFeatures.Queries.GetPagedProducts
{
    public class GetPagedProductsQueryHandler : IRequestHandler<GetPagedProductsQueryRequest, GetPagedProductsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPagedProductsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetPagedProductsQueryResponse> Handle(GetPagedProductsQueryRequest request, CancellationToken cancellationToken)
        {
            Expression<Func<Product, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(request.Search))
                filter = p => p.Name.Contains(request.Search);

            var pagedResult = await _unitOfWork.Repositories<Product>()
                .Search(
                    filter,
                    request.Page,
                    request.PageSize,
                    null,
                    p => p.AttributeValues
                );

            var dtoResult = new PagedResult<GetProductDto>
            {
                Items = pagedResult.Items.Select(p => p.ToDto()),
                TotalCount = pagedResult.TotalCount
            };

            return new GetPagedProductsQueryResponse
            {
                Result = dtoResult
            };
        }
    }
}