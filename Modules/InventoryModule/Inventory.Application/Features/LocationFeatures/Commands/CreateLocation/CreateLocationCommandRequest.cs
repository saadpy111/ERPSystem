using Inventory.Application.Dtos.LocationDtos;
using MediatR;

namespace Inventory.Application.Features.LocationFeatures.Commands.CreateLocation
{
    public class CreateLocationCommandRequest : IRequest<CreateLocationCommandResponse>
    {
        public CreateLocationDto Location { get; set; }
    }
}