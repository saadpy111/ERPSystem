using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.StockMoveDtos;
using Inventory.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Inventory.Application.Features.StockMoveFeatures.Queries.GetAllStockMoves
{
    public class GetAllStockMovesQueryHandler : IRequestHandler<GetAllStockMovesQueryRequest, GetAllStockMovesQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllStockMovesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAllStockMovesQueryResponse> Handle(GetAllStockMovesQueryRequest request, CancellationToken cancellationToken)
        {
            Expression<Func<StockMove, bool>>? filter = null;
            if (request.ProductId.HasValue)
                filter = sm => sm.ProductId == request.ProductId.Value;

            var moves = await _unitOfWork.Repositories<StockMove>().GetAll(filter);

            var dtos = moves.Select(sm => new GetStockMoveDto
            {
                Id = sm.Id,
                Quantity = sm.Quantity,
                MoveDate = sm.MoveDate,
                Reference = sm.Reference,
                MoveType = sm.MoveType,
                ProductId = sm.ProductId,
                SourceLocationId = sm.SourceLocationId,
                DestinationLocationId = sm.DestinationLocationId,
                CreatedAt = sm.CreatedAt,
                UpdatedAt = sm.UpdatedAt
            }).ToList();

            return new GetAllStockMovesQueryResponse { StockMoves = dtos };
        }
    }
}