using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class ProductBarcode : BaseEntity
    {
        public string BarcodeValue { get; set; }
        public string Type { get; set; } // e.g. EAN13, EAN8, UPC, Code128

        /// <summary>
        /// Indicates this barcode encodes weight (e.g. GS1 prefix "21").
        /// POS will parse embedded weight from the barcode value.
        /// </summary>
        public bool IsWeighted { get; set; }

        /// <summary>
        /// Barcode prefix used for weighted items (e.g. "21", "22").
        /// Only meaningful when IsWeighted = true.
        /// </summary>
        public string? Prefix { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }

}
