using MediatR;

namespace Inventory.Application.Features.InventoryQuarantineFeatures.Queries.GetInventoryQuarantineById
{
    public class GetInventoryQuarantineByIdQueryRequest : IRequest<GetInventoryQuarantineByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}