using MediatR;
using Inventory.Application.Dtos.WarehouseDtos;

namespace Inventory.Application.Features.WarehouseFeatures.Commands.UpdateWarehouse
{
    public class UpdateWarehouseCommandRequest : IRequest<UpdateWarehouseCommandResponse>
    {
        public UpdateWarehouseDto Warehouse { get; set; }
    }
}