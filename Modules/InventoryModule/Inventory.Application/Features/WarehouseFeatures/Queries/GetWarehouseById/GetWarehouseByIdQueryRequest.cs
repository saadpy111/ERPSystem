using MediatR;
using Inventory.Application.Dtos.WarehouseDtos;

namespace Inventory.Application.Features.WarehouseFeatures.Queries.GetWarehouseById
{
    public class GetWarehouseByIdQueryRequest : IRequest<GetWarehouseByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}