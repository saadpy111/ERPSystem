namespace Website.Application.Features.WebsiteCategoryFeatures.Commands.UpdateCategoryImage
{
    public class UpdateWebsiteCategoryImageCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ImagePath { get; set; }
    }
}
