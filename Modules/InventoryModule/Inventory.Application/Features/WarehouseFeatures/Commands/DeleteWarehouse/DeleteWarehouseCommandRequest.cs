using MediatR;

namespace Inventory.Application.Features.WarehouseFeatures.Commands.DeleteWarehouse
{
    public class DeleteWarehouseCommandRequest : IRequest<DeleteWarehouseCommandResponse>
    {
        public Guid Id { get; set; }
    }
}