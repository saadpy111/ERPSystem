using MediatR;

namespace Inventory.Application.Features.StockAdjustmentFeatures.Queries.GetStockAdjustmentById
{
    public class GetStockAdjustmentByIdQueryRequest : IRequest<GetStockAdjustmentByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}