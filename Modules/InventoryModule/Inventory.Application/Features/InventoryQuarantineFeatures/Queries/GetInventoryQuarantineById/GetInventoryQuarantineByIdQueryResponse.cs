using Inventory.Application.Dtos.InventoryQuarantineDtos;

namespace Inventory.Application.Features.InventoryQuarantineFeatures.Queries.GetInventoryQuarantineById
{
    public class GetInventoryQuarantineByIdQueryResponse
    {
        public GetInventoryQuarantineDto? InventoryQuarantine { get; set; }
    }
}