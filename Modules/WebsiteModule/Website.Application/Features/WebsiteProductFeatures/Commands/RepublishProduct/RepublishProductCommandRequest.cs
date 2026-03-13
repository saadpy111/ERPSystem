using MediatR;

namespace Website.Application.Features.WebsiteProductFeatures.Commands.RepublishProduct
{
    public class RepublishProductCommandRequest : IRequest<RepublishProductCommandResponse>
    {
        public Guid WebsiteProductId { get; set; }
    }
}
