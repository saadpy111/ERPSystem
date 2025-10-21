using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.StockQuantDtos;
using Inventory.Application.Pagination;
using Inventory.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Inventory.Application.Features.StockQuantFeatures.Queries.GetPagedStockQuants
{
    public class GetPagedStockQuantsQueryHandler : IRequestHandler<GetPagedStockQuantsQueryRequest, GetPagedStockQuantsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPagedStockQuantsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetPagedStockQuantsQueryResponse> Handle(GetPagedStockQuantsQueryRequest request, CancellationToken cancellationToken)
        {
            Expression<Func<StockQuant, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(request.Search))
                filter = q => q.ProductId.ToString().Contains(request.Search) ||
                              q.LocationId.ToString().Contains(request.Search);

            var pagedResult = await _unitOfWork.Repositories<StockQuant>()
                .Search(filter, request.Page, request.PageSize);

            var dtoResult = new PagedResult<GetStockQuantDto>
            {
                Items = pagedResult.Items.Select(q => new GetStockQuantDto
                {
                    Id = q.Id,
                    ProductId = q.ProductId,
                    LocationId = q.LocationId,
                    Quantity = q.Quantity,
                    CreatedAt = q.CreatedAt,
                    UpdatedAt = q.UpdatedAt
                }),
                TotalCount = pagedResult.TotalCount
            };

            return new GetPagedStockQuantsQueryResponse
            {
                Result = dtoResult
            };
        }
    }
}