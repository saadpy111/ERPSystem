using MediatR;

namespace Website.Application.Features.CartFeatures.Commands.RemoveCartItem
{
    public class RemoveCartItemCommandRequest : IRequest<RemoveCartItemCommandResponse>
    {
        public Guid ItemId { get; set; }
    }
}
