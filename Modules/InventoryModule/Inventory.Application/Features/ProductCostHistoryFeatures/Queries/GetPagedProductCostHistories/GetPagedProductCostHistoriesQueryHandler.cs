using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductCostHistoryDtos;
using Inventory.Application.Pagination;
using Inventory.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Inventory.Application.Features.ProductCostHistoryFeatures.Queries.GetPagedProductCostHistories
{
    public class GetPagedProductCostHistoriesQueryHandler : IRequestHandler<GetPagedProductCostHistoriesQueryRequest, GetPagedProductCostHistoriesQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPagedProductCostHistoriesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetPagedProductCostHistoriesQueryResponse> Handle(GetPagedProductCostHistoriesQueryRequest request, CancellationToken cancellationToken)
        {
            Expression<Func<ProductCostHistory, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(request.Search))
                filter = h => h.ProductId.ToString().Contains(request.Search);

            var pagedResult = await _unitOfWork.Repositories<ProductCostHistory>()
                .Search(filter, request.Page, request.PageSize);

            var dtoResult = new PagedResult<GetProductCostHistoryDto>
            {
                Items = pagedResult.Items.Select(h => new GetProductCostHistoryDto
                {
                    Id = h.Id,
                    OldCost = h.OldCost,
                    NewCost = h.NewCost,
                    ProductId = h.ProductId,
                    CreatedAt = h.CreatedAt
                }),
                TotalCount = pagedResult.TotalCount
            };

            return new GetPagedProductCostHistoriesQueryResponse
            {
                Result = dtoResult
            };
        }
    }
}