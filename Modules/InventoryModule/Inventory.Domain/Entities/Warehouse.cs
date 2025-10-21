using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class Warehouse : BaseEntity
    {
        public string Name { get; set; }
        public string LocationDetails { get; set; }
        public ICollection<Location> Locations { get; set; }
        public ICollection<StockAdjustment> StockAdjustments { get; set; }
    }
}
