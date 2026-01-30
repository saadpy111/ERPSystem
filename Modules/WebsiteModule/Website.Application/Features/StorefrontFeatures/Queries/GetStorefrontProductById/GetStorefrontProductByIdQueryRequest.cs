using MediatR;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontProductById
{
    public class GetStorefrontProductByIdQueryRequest : IRequest<GetStorefrontProductByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
