using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class ProductCategory : BaseEntity
    {
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public ProductCategory ParentCategory { get; set; }
        public ICollection<ProductCategory> ChildCategories { get; set; }
        public ICollection<Product> Products { get; set; }
        public string? ImagePath { get; set; }
    }
}
