using MediatR;

namespace Inventory.Application.Features.StockQuantFeatures.Queries.GetStockQuantById
{
    public class GetStockQuantByIdQueryRequest : IRequest<GetStockQuantByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}