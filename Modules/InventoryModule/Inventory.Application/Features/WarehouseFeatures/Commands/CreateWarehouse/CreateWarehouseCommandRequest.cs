using MediatR;
using Inventory.Application.Dtos.WarehouseDtos;

namespace Inventory.Application.Features.WarehouseFeatures.Commands.CreateWarehouse
{
    public class CreateWarehouseCommandRequest : IRequest<CreateWarehouseCommandResponse>
    {
        public CreateWarehouseDto Warehouse { get; set; }
    }
}