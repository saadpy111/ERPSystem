using MediatR;

namespace Website.Application.Features.WebsiteProductFeatures.Queries.GetProductById
{
    public class GetProductByIdQueryRequest : IRequest<GetProductByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
