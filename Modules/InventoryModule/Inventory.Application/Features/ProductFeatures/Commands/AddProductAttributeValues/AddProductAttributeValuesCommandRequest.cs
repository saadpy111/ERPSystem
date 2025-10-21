using Inventory.Application.Dtos.ProductDtos;
using MediatR;
using System;
using System.Collections.Generic;

namespace Inventory.Application.Features.ProductFeatures.Commands.AddProductAttributeValues
{
    public class AddProductAttributeValuesCommandRequest : IRequest<AddProductAttributeValuesCommandResponse>
    {
        public Guid ProductId { get; set; }
        public List<ProductAttributeValueDto> AttributeValues { get; set; }
    }
}