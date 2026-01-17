using MediatR;

namespace Website.Application.Features.WebsiteProductFeatures.Commands.UnpublishProduct
{
    public class UnpublishProductCommandRequest : IRequest<UnpublishProductCommandResponse>
    {
        public Guid WebsiteProductId { get; set; }
    }
}
