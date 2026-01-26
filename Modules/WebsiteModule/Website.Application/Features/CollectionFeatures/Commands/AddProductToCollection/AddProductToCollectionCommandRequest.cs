using MediatR;

namespace Website.Application.Features.CollectionFeatures.Commands.AddProductToCollection
{
    public class AddProductToCollectionCommandRequest : IRequest<AddProductToCollectionCommandResponse>
    {
        public Guid CollectionId { get; set; }
        public Guid ProductId { get; set; }
        public int DisplayOrder { get; set; } = 0;
    }
}
