using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class ProductBarcode : BaseEntity
    {
        public string BarcodeValue { get; set; }
        public string Type { get; set; } 

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }

}
