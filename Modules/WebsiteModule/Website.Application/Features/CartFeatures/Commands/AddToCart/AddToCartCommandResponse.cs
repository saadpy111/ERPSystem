namespace Website.Application.Features.CartFeatures.Commands.AddToCart
{
    public class AddToCartCommandResponse
    {
        public bool Success { get; set; }
        public Guid? CartId { get; set; }
        public string? Message { get; set; }
    }
}
