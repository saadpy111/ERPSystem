using MediatR;

namespace Website.Application.Features.CollectionFeatures.Commands.RemoveProductFromCollection
{
    public class RemoveProductFromCollectionCommandRequest : IRequest<RemoveProductFromCollectionCommandResponse>
    {
        public Guid CollectionId { get; set; }
        public Guid ProductId { get; set; }
    }
}
