using MediatR;

namespace Website.Application.Features.CartFeatures.Commands.AddToCart
{
    public class AddToCartCommandRequest : IRequest<AddToCartCommandResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
