using Inventory.Application.Dtos.LocationDtos;

namespace Inventory.Application.Features.LocationFeatures.Commands.UpdateLocation
{
    public class UpdateLocationCommandResponse
    {
        public bool Success { get; set; }
        public GetLocationDto? Location { get; set; }
    }
}