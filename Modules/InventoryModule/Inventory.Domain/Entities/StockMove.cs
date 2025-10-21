using Inventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class StockMove : BaseEntity
    {
        public int Quantity { get; set; }
        public DateTime MoveDate { get; set; }
        public string? Reference { get; set; } 
        public StockMoveType MoveType { get; set; } 

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid? SourceLocationId { get; set; }
        public Location SourceLocation { get; set; }

        public Guid? DestinationLocationId { get; set; }
        public Location DestinationLocation { get; set; }
    }
}
