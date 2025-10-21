using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class ProductAttributeValue : BaseEntity
    {
        public string Value { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid AttributeId { get; set; }
        public ProductAttribute Attribute { get; set; }
    }
}
