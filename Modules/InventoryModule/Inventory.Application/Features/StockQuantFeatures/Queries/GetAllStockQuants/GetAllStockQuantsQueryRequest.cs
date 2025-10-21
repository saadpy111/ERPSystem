using MediatR;

namespace Inventory.Application.Features.StockQuantFeatures.Queries.GetAllStockQuants
{
    public class GetAllStockQuantsQueryRequest : IRequest<GetAllStockQuantsQueryResponse>
    {
        public Guid? ProductId { get; set; }
        public Guid? LocationId { get; set; }
    }
}