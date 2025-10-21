using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class ProductAttribute : BaseEntity
    {
        public string Name { get; set; } 
        public string DataType { get; set; }

        public ICollection<ProductAttributeValue> Values { get; set; }
    }
}

