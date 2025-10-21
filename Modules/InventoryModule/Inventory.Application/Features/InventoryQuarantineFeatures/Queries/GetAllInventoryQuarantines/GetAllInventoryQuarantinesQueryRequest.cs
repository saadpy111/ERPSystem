using Inventory.Domain.Enums;
using MediatR;

namespace Inventory.Application.Features.InventoryQuarantineFeatures.Queries.GetAllInventoryQuarantines
{
    public class GetAllInventoryQuarantinesQueryRequest : IRequest<GetAllInventoryQuarantinesQueryResponse>
    {
        public Guid? ProductId { get; set; }
        public Guid? LocationId { get; set; }
        public QuarantineStatus? Status { get; set; }
    }
}