using Microsoft.AspNetCore.Http;
using SharedKernel.Multitenancy;

namespace SharedKernel.Middleware
{
    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantResolutionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantProvider tenantProvider)
        {
            var tenantClaim = context.User.FindFirst("tenant");

            if (tenantClaim != null)
            {
                tenantProvider.SetTenantId(tenantClaim.Value);
            }

            await _next(context);
        }
    }

}
