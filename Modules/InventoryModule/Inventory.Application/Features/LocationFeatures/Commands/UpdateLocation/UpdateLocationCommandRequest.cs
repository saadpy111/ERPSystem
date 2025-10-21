using Inventory.Application.Dtos.LocationDtos;
using MediatR;

namespace Inventory.Application.Features.LocationFeatures.Commands.UpdateLocation
{
    public class UpdateLocationCommandRequest : IRequest<UpdateLocationCommandResponse>
    {
        public UpdateLocationDto Location { get; set; }
    }
}