using MediatR;

namespace Website.Application.Features.OfferFeatures.Queries.GetOfferById
{
    public class GetOfferByIdQueryRequest : IRequest<GetOfferByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
