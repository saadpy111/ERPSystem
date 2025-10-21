using Inventory.Application.Dtos.InventoryQuarantineDtos;
using System.Collections.Generic;

namespace Inventory.Application.Features.InventoryQuarantineFeatures.Queries.GetAllInventoryQuarantines
{
    public class GetAllInventoryQuarantinesQueryResponse
    {
        public List<GetInventoryQuarantineDto> InventoryQuarantines { get; set; } = new();
    }
}