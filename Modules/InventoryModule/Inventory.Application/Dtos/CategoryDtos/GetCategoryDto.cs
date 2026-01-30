using Inventory.Domain.Entities;
using SharedKernel.Core.Files;

namespace Inventory.Application.Dtos.CategoryDtos
{
    public class GetCategoryDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public List<GetChildCategoryDto>? Children { get; set; }
    }

    public static class GetCategoryExtentions
    {
        public static GetCategoryDto ToDto(this ProductCategory category, IFileUrlResolver urlResolver)
        {
            if (category == null)
                return new GetCategoryDto();


            return new GetCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = urlResolver.Resolve(category.ImagePath),
                Children = category.ChildCategories?
                                .Select(c => new GetChildCategoryDto()
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    ImageUrl = urlResolver.Resolve(c.ImagePath)
                                }).ToList()
            };
            
        }
    }
}
