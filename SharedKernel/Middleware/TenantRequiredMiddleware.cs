using Microsoft.AspNetCore.Http;
using SharedKernel.Authorization;
using SharedKernel.Multitenancy;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedKernel.Middleware
{
    /// <summary>
    /// Middleware to enforce tenant requirement.
    /// Denies access (403) if user doesn't have a tenant claim.
    /// Apply [RequiresTenant] attribute on endpoints that need tenant (checked via route/action patterns).
    /// </summary>
    public class TenantRequiredMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantRequiredMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantProvider tenantProvider)
        {
            var endpoint = context.GetEndpoint();
            var requiresTenant = endpoint?.Metadata.GetMetadata<RequiresTenantAttribute>();

            if (requiresTenant != null)
            {
                var tenantId = tenantProvider.GetTenantId();

                if (string.IsNullOrEmpty(tenantId))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("TENANT_REQUIRED");
                    return;
                }
            }

            await _next(context);
        }
    }
}
