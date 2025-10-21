using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class StockQuant : BaseEntity
    {
        public int Quantity { get; set; }
        public int ReservedQuantity { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid LocationId { get; set; }
        public Location Location { get; set; }
    }
}
