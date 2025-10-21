using MediatR;

namespace Inventory.Application.Features.LocationFeatures.Queries.GetAllLocations
{
    public class GetAllLocationsQueryRequest : IRequest<GetAllLocationsQueryResponse>
    {
        public Guid? WarehouseId { get; set; }
    }
}