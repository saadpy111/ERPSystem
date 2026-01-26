using MediatR;

namespace Website.Application.Features.OfferFeatures.Commands.DeleteOffer
{
    public class DeleteOfferCommandRequest : IRequest<DeleteOfferCommandResponse>
    {
        public Guid Id { get; set; }
    }
}
