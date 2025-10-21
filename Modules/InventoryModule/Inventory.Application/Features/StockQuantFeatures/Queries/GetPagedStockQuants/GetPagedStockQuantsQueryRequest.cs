using MediatR;

namespace Inventory.Application.Features.StockQuantFeatures.Queries.GetPagedStockQuants
{
    public class GetPagedStockQuantsQueryRequest : IRequest<GetPagedStockQuantsQueryResponse>
    {
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}