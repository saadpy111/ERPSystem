using MediatR;
using Website.Application.Pagination;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontOffers
{
    public class GetStorefrontOffersQueryRequest : IRequest<GetStorefrontOffersQueryResponse>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
