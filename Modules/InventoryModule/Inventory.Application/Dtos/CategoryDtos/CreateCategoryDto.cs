using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Dtos.CategoryDtos
{
    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
    }
    public static class CreateCategoryExtentions
    {
        public static ProductCategory ToEntity(this CreateCategoryDto dto)
            => new ProductCategory()
            {
                   Id = Guid.NewGuid(),
                   Name = dto.Name,
                   ParentId = dto.ParentId,
                   CreatedAt =  DateTime.UtcNow
            };
    }
}
