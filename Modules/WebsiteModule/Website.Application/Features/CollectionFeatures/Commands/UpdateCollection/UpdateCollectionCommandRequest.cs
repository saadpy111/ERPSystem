using MediatR;

namespace Website.Application.Features.CollectionFeatures.Commands.UpdateCollection
{
    public class UpdateCollectionCommandRequest : IRequest<UpdateCollectionCommandResponse>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool? IsActive { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
