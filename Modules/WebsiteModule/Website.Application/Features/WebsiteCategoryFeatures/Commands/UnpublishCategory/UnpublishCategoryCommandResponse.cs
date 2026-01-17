namespace Website.Application.Features.WebsiteCategoryFeatures.Commands.UnpublishCategory
{
    public class UnpublishCategoryCommandResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int ProductsAffected { get; set; }
    }
}
