using MediatR;

namespace Inventory.Application.Features.InventoryQuarantineFeatures.Queries.GetPagedInventoryQuarantines
{
    public class GetPagedInventoryQuarantinesQueryRequest : IRequest<GetPagedInventoryQuarantinesQueryResponse>
    {
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}