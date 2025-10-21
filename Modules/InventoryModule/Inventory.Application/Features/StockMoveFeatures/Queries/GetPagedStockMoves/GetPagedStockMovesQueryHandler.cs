using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.StockMoveDtos;
using Inventory.Application.Pagination;
using Inventory.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Inventory.Application.Features.StockMoveFeatures.Queries.GetPagedStockMoves
{
    public class GetPagedStockMovesQueryHandler : IRequestHandler<GetPagedStockMovesQueryRequest, GetPagedStockMovesQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPagedStockMovesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetPagedStockMovesQueryResponse> Handle(GetPagedStockMovesQueryRequest request, CancellationToken cancellationToken)
        {
            Expression<Func<StockMove, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(request.Search))
                filter = m => m.Reference.Contains(request.Search);

            var pagedResult = await _unitOfWork.Repositories<StockMove>()
                .Search(
                    filter,
                    request.Page,
                    request.PageSize
                );

            var dtoResult = new PagedResult<GetStockMoveDto>
            {
                Items = pagedResult.Items.Select(m => new GetStockMoveDto
                {
                    Id = m.Id,
                    Quantity = m.Quantity,
                    MoveDate = m.MoveDate,
                    Reference = m.Reference,
                    MoveType = m.MoveType,
                    ProductId = m.ProductId,
                    SourceLocationId = m.SourceLocationId.Value,
                    DestinationLocationId = m.DestinationLocationId.Value,
                    CreatedAt = m.CreatedAt,
                    UpdatedAt = m.UpdatedAt
                }),
                TotalCount = pagedResult.TotalCount
            };

            return new GetPagedStockMovesQueryResponse
            {
                Result = dtoResult
            };
        }
    }
}