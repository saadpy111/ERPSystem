namespace Subscription.Application.Features.Subscriptions.Queries.GetTenantWebsiteDetails
{
    public class GetTenantWebsiteDetailsResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public TenantWebsiteDetailsDto? Data { get; set; }
    }

    public class TenantWebsiteDetailsDto
    {
        public bool HasWebsite { get; set; }
        public string? Domain { get; set; }
        public string? Name { get; set; }
        public bool IsPublished { get; set; }
    }
}
