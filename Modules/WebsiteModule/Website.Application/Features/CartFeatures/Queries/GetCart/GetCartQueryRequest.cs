using MediatR;

namespace Website.Application.Features.CartFeatures.Queries.GetCart
{
    public class GetCartQueryRequest : IRequest<GetCartQueryResponse>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
