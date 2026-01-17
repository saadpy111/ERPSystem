namespace Website.Application.Features.CollectionFeatures.Commands.CreateCollection
{
    public class CreateCollectionCommandResponse
    {
        public bool Success { get; set; }
        public Guid? CollectionId { get; set; }
        public string? Message { get; set; }
    }
}
