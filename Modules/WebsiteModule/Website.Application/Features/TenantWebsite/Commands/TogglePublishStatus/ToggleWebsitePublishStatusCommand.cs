using MediatR;

namespace Website.Application.Features.TenantWebsite.Commands.TogglePublishStatus
{
    public class ToggleWebsitePublishStatusCommand : IRequest<ToggleWebsitePublishStatusResponse>
    {
        public string TenantId { get; set; } = string.Empty;
    }

    public class ToggleWebsitePublishStatusResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public bool NewPublishStatus { get; set; }
    }
}
