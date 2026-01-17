using MediatR;

namespace Website.Application.Features.CollectionFeatures.Commands.CreateCollection
{
    public class CreateCollectionCommandRequest : IRequest<CreateCollectionCommandResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int DisplayOrder { get; set; } = 0;
    }
}
