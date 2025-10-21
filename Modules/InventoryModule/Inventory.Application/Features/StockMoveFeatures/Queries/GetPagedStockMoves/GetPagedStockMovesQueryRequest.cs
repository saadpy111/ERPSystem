using MediatR;

namespace Inventory.Application.Features.StockMoveFeatures.Queries.GetPagedStockMoves
{
    public class GetPagedStockMovesQueryRequest : IRequest<GetPagedStockMovesQueryResponse>
    {
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}