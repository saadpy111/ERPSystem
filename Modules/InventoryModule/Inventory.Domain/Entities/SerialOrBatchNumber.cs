using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    
public class SerialOrBatchNumber : BaseEntity
    {
        public string Number { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int Quantity { get; set; } 

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid? CurrentLocationId { get; set; }
        public Location CurrentLocation { get; set; }
    }
}
