using MediatR;

namespace Website.Application.Features.CollectionFeatures.Commands.DeleteCollection
{
    public class DeleteCollectionCommandRequest : IRequest<DeleteCollectionCommandResponse>
    {
        public Guid Id { get; set; }
    }
}
