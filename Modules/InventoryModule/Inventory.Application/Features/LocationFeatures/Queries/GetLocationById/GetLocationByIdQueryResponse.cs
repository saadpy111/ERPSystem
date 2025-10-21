using Inventory.Application.Dtos.LocationDtos;

namespace Inventory.Application.Features.LocationFeatures.Queries.GetLocationById
{
    public class GetLocationByIdQueryResponse
    {
        public GetLocationDto? Location { get; set; }
    }
}