using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Enums
{
    public enum StockMoveType
    {
        Purchase = 1,   
        Sale = 2,       
        Transfer = 3,   
        Adjustment = 4  ,
        Return = 5,
        UnderReview = 6
    }
}
