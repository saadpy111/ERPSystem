using MediatR;

namespace Website.Application.Features.TenantWebsite.Queries.GetPublishStatus
{
    public class GetWebsitePublishStatusQuery : IRequest<GetWebsitePublishStatusResponse>
    {
        public string TenantId { get; set; } = string.Empty;
    }

    public class GetWebsitePublishStatusResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public bool IsPublished { get; set; }
    }
}
