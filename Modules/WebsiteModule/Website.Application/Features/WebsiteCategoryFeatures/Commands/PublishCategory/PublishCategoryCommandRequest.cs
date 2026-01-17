using MediatR;

namespace Website.Application.Features.WebsiteCategoryFeatures.Commands.PublishCategory
{
    public class PublishCategoryCommandRequest : IRequest<PublishCategoryCommandResponse>
    {
        public Guid? InventoryCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public int DisplayOrder { get; set; } = 0;
    }
}
