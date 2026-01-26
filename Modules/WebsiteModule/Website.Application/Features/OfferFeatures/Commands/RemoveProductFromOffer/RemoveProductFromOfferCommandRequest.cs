using MediatR;

namespace Website.Application.Features.OfferFeatures.Commands.RemoveProductFromOffer
{
    public class RemoveProductFromOfferCommandRequest : IRequest<RemoveProductFromOfferCommandResponse>
    {
        public Guid OfferId { get; set; }
        public Guid ProductId { get; set; }
    }
}
