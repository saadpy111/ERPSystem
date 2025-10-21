using Inventory.Application.Pagination;
using Inventory.Application.Dtos.InventoryQuarantineDtos;

namespace Inventory.Application.Features.InventoryQuarantineFeatures.Queries.GetPagedInventoryQuarantines
{
    public class GetPagedInventoryQuarantinesQueryResponse
    {
        public PagedResult<GetInventoryQuarantineDto> Result { get; set; }
    }
}