using MediatR;
using Inventory.Application.Dtos.WarehouseDtos;
using System.Collections.Generic;

namespace Inventory.Application.Features.WarehouseFeatures.Queries.GetAllWarehouses
{
    public class GetAllWarehousesQueryRequest : IRequest<GetAllWarehousesQueryResponse> { }
}