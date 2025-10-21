using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class Location : BaseEntity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public Location  Parent { get; set; }
        public Guid? ParentId { get; set; } // Self-Reference
        public ICollection<Location> ChildLocations { get; set; }
        public ICollection<StockQuant> StockQuants { get; set; }
    }
}
