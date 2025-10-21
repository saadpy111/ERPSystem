using Inventory.Application.Dtos.ProductAttributeDtos;
using MediatR;

namespace Inventory.Application.Features.ProductAttributeFeatures.Queries.GetProductAttributeById
{
    public class GetProductAttributeByIdQueryRequest : IRequest<GetProductAttributeByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}