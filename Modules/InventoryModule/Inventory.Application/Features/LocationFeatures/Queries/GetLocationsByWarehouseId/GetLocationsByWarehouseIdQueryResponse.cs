using Inventory.Application.Dtos.LocationDtos;
using System.Collections.Generic;

namespace Inventory.Application.Features.LocationFeatures.Queries.GetLocationsByWarehouseId
{
    public class GetLocationsByWarehouseIdQueryResponse
    {
        public List<GetLocationDto> Locations { get; set; } = new();
    }
}