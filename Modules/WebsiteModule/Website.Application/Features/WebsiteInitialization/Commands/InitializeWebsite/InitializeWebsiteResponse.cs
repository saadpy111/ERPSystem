namespace Website.Application.Features.WebsiteInitialization.Commands.InitializeWebsite
{
    public class InitializeWebsiteResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public Guid? WebsiteId { get; set; }
    }
}
