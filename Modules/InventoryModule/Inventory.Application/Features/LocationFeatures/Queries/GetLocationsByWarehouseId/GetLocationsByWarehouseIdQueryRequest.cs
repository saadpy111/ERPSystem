using MediatR;
using System;

namespace Inventory.Application.Features.LocationFeatures.Queries.GetLocationsByWarehouseId
{
    public class GetLocationsByWarehouseIdQueryRequest : IRequest<GetLocationsByWarehouseIdQueryResponse>
    {
        public Guid WarehouseId { get; set; }
    }
}