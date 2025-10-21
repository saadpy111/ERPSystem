using Inventory.Application.Dtos.CategoryDtos;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.ProCategoryFeatures.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryResponse
    {
        public  GetCategoryDto    CategoryDto { get; set; }
    }


}
