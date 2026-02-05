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
       ITenantDomainResolver tenantDomainResolver)
        {
            string? tenantFromToken = context.User?.FindFirst("tenant")?.Value;
            string? tenantFromHeader = null;

            if (context.Request.Headers.TryGetValue("X-Tenant-Key", out var tenantKey))
            {
               
                var tenantResult = await tenantDomainResolver.GetTenantByDomainAsync(tenantKey!);
                tenantFromHeader = tenantResult?.TenantId;
            }

            if (!string.IsNullOrEmpty(tenantFromToken) &&
                !string.IsNullOrEmpty(tenantFromHeader) &&
                tenantFromToken != tenantFromHeader)
            {
               
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Cross-tenant access is not allowed. Please login to the target tenant.");
                return;
            }

      
            string? finalTenantId = tenantFromToken ?? tenantFromHeader;

            if (!string.IsNullOrEmpty(finalTenantId))
            {
                tenantProvider.SetTenantId(finalTenantId);
            }
            else
            {
                // (Optional) If tenant is mandatory, return 400 Bad Request
            }

            await _next(context);
        }
    }
}
