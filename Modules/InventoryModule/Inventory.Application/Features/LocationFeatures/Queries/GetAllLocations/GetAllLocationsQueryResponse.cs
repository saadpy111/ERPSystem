using Inventory.Application.Dtos.LocationDtos;

namespace Inventory.Application.Features.LocationFeatures.Queries.GetAllLocations
{
    public class GetAllLocationsQueryResponse
    {
        public List<GetLocationDto> Locations { get; set; } = new();
    }
}