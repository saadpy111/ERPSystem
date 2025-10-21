using Inventory.Application.Dtos.LocationDtos;

namespace Inventory.Application.Features.LocationFeatures.Commands.CreateLocation
{
    public class CreateLocationCommandResponse
    {
        public bool Success { get; set; }
        public GetLocationDto? Location { get; set; }
    }
}