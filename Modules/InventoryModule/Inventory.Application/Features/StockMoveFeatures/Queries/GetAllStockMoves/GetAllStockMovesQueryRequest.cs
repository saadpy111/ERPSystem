using MediatR;

namespace Inventory.Application.Features.StockMoveFeatures.Queries.GetAllStockMoves
{
    public class GetAllStockMovesQueryRequest : IRequest<GetAllStockMovesQueryResponse>
    {
        public Guid? ProductId { get; set; }
    }
}