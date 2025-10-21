using MediatR;

namespace Inventory.Application.Features.LocationFeatures.Queries.GetLocationById
{
    public class GetLocationByIdQueryRequest : IRequest<GetLocationByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}