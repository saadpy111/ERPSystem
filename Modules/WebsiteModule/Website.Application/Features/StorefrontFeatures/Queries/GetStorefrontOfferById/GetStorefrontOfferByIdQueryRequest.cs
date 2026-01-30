using MediatR;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontOfferById
{
    public class GetStorefrontOfferByIdQueryRequest : IRequest<GetStorefrontOfferByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
