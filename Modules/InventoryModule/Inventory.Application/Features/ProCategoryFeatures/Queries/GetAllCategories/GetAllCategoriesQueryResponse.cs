
using Inventory.Application.Dtos.CategoryDtos;

namespace Inventory.Application.Features.ProCategoryFeatures.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryResponse
    {
        public List<GetCategoryDto>?  Categories { get; set; }
    }
}
