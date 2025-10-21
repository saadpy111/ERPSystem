using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.StockAdjustmentDtos;
using Inventory.Application.Pagination;
using Inventory.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Inventory.Application.Features.StockAdjustmentFeatures.Queries.GetPagedStockAdjustments
{
    public class GetPagedStockAdjustmentsQueryHandler : IRequestHandler<GetPagedStockAdjustmentsQueryRequest, GetPagedStockAdjustmentsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPagedStockAdjustmentsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetPagedStockAdjustmentsQueryResponse> Handle(GetPagedStockAdjustmentsQueryRequest request, CancellationToken cancellationToken)
        {
            Expression<Func<StockAdjustment, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(request.Search))
                filter = a => a.ProductId.ToString().Contains(request.Search) ||
                              a.WarehouseId.ToString().Contains(request.Search);

            var pagedResult = await _unitOfWork.Repositories<StockAdjustment>()
                .Search(filter, request.Page, request.PageSize);

            var dtoResult = new PagedResult<GetStockAdjustmentDto>
            {
                Items = pagedResult.Items.Select(a => new GetStockAdjustmentDto
                {
                    Id = a.Id,
                    ExpectedQuantity = a.ExpectedQuantity,
                    ActualQuantity = a.ActualQuantity,
                    Date = a.Date,
                    UserId = a.UserId,
                    WarehouseId = a.WarehouseId,
                    ProductId = a.ProductId,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                }),
                TotalCount = pagedResult.TotalCount
            };

            return new GetPagedStockAdjustmentsQueryResponse
            {
                Result = dtoResult
            };
        }
    }
}