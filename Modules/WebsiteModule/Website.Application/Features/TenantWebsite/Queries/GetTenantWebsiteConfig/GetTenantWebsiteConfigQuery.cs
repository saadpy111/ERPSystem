using MediatR;
using Website.Application.DTOs;

namespace Website.Application.Features.TenantWebsite.Queries.GetTenantWebsiteConfig
{
    public class GetTenantWebsiteConfigQuery : IRequest<GetTenantWebsiteConfigResponse>
    {
        public string TenantId { get; set; } = string.Empty;
    }

    public class GetTenantWebsiteConfigResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public TenantWebsiteDto? Config { get; set; }
    }
}
