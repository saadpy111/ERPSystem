using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.Entities
{
    public class InventoryReport : BaseEntity
    {
        public int InventoryReportId { get; set; }

        //  Product Info
        public string ProductName { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public string? MainSupplierName { get; set; }
        public decimal? Tax { get; set; }
        public int? OrderLimit { get; set; }


        //  Warehouse Info
        public string WarehouseName { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;

        //  Quantities
        public decimal? AvailableQuantity { get; set; }  
        public decimal? ReservedQuantity { get; set; }   
        public decimal? QuarantineQuantity { get; set; } 
        public decimal? TotalQuantity { get; set; }

        //  Cost & Valuation
        public string? UnitOfMeasure { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? CostPrice { get; set; }

        public decimal? UnitCost { get; set; }
        public decimal? TotalCost { get; set; }

        //  Stock Status
        public bool IsLowStock { get; set; }
        public bool IsOutOfStock { get; set; }

        //  Last Movement
        public DateTime? LastStockMoveDate { get; set; }
        public string LastStockMoveType { get; set; } = string.Empty;


        //  Audit
        public DateTime CreatedAt { get; set; }
    }

}



