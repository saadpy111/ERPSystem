namespace Website.Application.Features.WebsiteCategoryFeatures.Commands.PublishCategory
{
    public class PublishCategoryCommandResponse
    {
        public bool Success { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Message { get; set; }
    }
}
