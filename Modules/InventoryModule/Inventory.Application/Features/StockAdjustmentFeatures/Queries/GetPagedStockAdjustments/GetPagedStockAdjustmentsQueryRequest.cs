using MediatR;

namespace Inventory.Application.Features.StockAdjustmentFeatures.Queries.GetPagedStockAdjustments
{
    public class GetPagedStockAdjustmentsQueryRequest : IRequest<GetPagedStockAdjustmentsQueryResponse>
    {
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}