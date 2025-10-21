using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class StockAdjustment : BaseEntity
    {
        public int ExpectedQuantity { get; set; }
        public int ActualQuantity { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; } 

        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
