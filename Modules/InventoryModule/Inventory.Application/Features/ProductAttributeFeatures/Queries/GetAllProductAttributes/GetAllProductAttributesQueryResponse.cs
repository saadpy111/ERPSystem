using Inventory.Application.Dtos.ProductAttributeDtos;
using System.Collections.Generic;

namespace Inventory.Application.Features.ProductAttributeFeatures.Queries.GetAllProductAttributes
{
    public class GetAllProductAttributesQueryResponse
    {
        public List<GetProductAttributeDto> ProductAttributes { get; set; }
    }
}