using MediatR;

namespace Website.Application.Features.OfferFeatures.Queries.GetAllOffers
{
    public class GetAllOffersQueryRequest : IRequest<GetAllOffersQueryResponse>
    {
        public bool? IsActive { get; set; }
    }
}
