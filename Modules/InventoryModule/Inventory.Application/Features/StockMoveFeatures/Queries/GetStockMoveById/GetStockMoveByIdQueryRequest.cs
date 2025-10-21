using MediatR;

namespace Inventory.Application.Features.StockMoveFeatures.Queries.GetStockMoveById
{
    public class GetStockMoveByIdQueryRequest : IRequest<GetStockMoveByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}