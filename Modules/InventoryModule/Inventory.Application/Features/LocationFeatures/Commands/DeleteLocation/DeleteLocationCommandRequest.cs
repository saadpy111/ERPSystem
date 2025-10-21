using MediatR;

namespace Inventory.Application.Features.LocationFeatures.Commands.DeleteLocation
{
    public class DeleteLocationCommandRequest : IRequest<DeleteLocationCommandResponse>
    {
        public Guid Id { get; set; }
    }
}