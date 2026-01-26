using MediatR;

namespace Website.Application.Features.OfferFeatures.Commands.AddProductToOffer
{
    public class AddProductToOfferCommandRequest : IRequest<AddProductToOfferCommandResponse>
    {
        public Guid OfferId { get; set; }
        public Guid ProductId { get; set; }
    }
}
