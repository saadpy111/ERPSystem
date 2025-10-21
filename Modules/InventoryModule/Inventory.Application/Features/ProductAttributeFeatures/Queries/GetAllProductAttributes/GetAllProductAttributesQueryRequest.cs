using Inventory.Application.Dtos.ProductAttributeDtos;
using MediatR;

namespace Inventory.Application.Features.ProductAttributeFeatures.Queries.GetAllProductAttributes
{
    public class GetAllProductAttributesQueryRequest : IRequest<GetAllProductAttributesQueryResponse> { }
}