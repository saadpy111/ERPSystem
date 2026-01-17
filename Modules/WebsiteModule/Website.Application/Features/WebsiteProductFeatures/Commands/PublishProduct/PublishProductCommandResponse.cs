namespace Website.Application.Features.WebsiteProductFeatures.Commands.PublishProduct
{
    public class PublishProductCommandResponse
    {
        public bool Success { get; set; }
        public Guid? ProductId { get; set; }
        public string? Message { get; set; }
    }
}
