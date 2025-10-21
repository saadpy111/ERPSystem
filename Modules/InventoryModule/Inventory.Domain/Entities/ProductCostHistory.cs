using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class ProductCostHistory : BaseEntity
    {
        public decimal OldCost { get; set; }
        public decimal NewCost { get; set; }
        public DateTime ChangeDate { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid? StockMoveId { get; set; } 
        public StockMove StockMove { get; set; }
    }
}
