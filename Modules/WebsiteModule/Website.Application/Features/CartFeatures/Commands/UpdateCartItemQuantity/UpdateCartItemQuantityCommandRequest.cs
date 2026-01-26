using MediatR;

namespace Website.Application.Features.CartFeatures.Commands.UpdateCartItemQuantity
{
    public class UpdateCartItemQuantityCommandRequest : IRequest<UpdateCartItemQuantityCommandResponse>
    {
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
