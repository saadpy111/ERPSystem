using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Dtos.ProductDtos
{
    public class UpdateProductAttributeValueDto
    {
        public Guid Id { get; set; }
        public Guid AttributeId { get; set; }
        public Guid ProductId { get; set; }
        public string Value { get; set; }
    }
}
