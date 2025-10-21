using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Dtos.CategoryDtos
{
    public class GetChildCategoryDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
