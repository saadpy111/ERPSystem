using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.InventoryEvents
{
    public class NewProductCreatedEvent:INotification
    {
        public string? Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public bool IsActive { get; set; }

        public string? ProductBarcode { get; set; }
        public string? MainSupplierName { get; set; }
        public decimal? Tax { get; set; }
        public int? OrderLimit { get; set; }

        public string CategoryName { get; set; }

    }
}
