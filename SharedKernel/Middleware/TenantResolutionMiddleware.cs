using Microsoft.AspNetCore.Http;
using SharedKernel.Multitenancy;
using SharedKernel.Website;

namespace SharedKernel.Middleware
{
    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantResolutionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            ITenantProvider tenantProvider,
            ITenantDomainResolver  tenantDomainResolver)
        {
            var tenantClaim = context.User?.FindFirst("tenant")?.Value;
            if (!string.IsNullOrEmpty(tenantClaim))
            {
                tenantProvider.SetTenantId(tenantClaim);
                await _next(context);
                return;
            }

            if (context.Request.Headers.TryGetValue("X-Tenant-Key", out var tenantKey))
            {
                var tenantResult = await tenantDomainResolver.GetTenantByDomainAsync(tenantKey!);
                var tenantId = tenantResult?.TenantId;
                if (!string.IsNullOrEmpty(tenantId))
                {
                    tenantProvider.SetTenantId(tenantId);
                }
            }

            await _next(context);
        }
    }
}
