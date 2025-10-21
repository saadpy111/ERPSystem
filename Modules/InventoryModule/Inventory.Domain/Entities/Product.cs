using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string? Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public bool IsActive { get; set; }

        // FK: ProductCategory
        public Guid CategoryId { get; set; }
        public ProductCategory Category { get; set; }

        public ICollection<StockQuant> StockQuants { get; set; }
        public ICollection<StockMove> StockMoves { get; set; }
        public ICollection<StockAdjustment> StockAdjustments { get; set; }
        public ICollection<SerialOrBatchNumber> SerialNumbers { get; set; }
        public ICollection<ProductCostHistory> CostHistories { get; set; }
        public ICollection<ProductBarcode> Barcodes { get; set; }
        public ICollection<ProductAttributeValue> AttributeValues { get; set; } 
        public ICollection<ProductImage> Images { get; set; }
    }
}
