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
        public string? WarehouseCode { get; set; }
        public string? ResponsibleEmployee { get; set; }
        public string? ContactNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public string? WarehouseType { get; set; }
        public string? FinancialAccountCode { get; set; }
        public decimal? PercentageUtilized { get; set; }
        public int? TotalStorageCapacity { get; set; }
        public string? InventoryPolicy { get; set; }
        public string? Government { get; set; }
        public ICollection<Location> Locations { get; set; }
        public ICollection<StockAdjustment> StockAdjustments { get; set; }
    }
}
