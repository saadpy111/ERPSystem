using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Api.Middleware
{
    public class VisitorTrackingMiddleware
    {
        private readonly RequestDelegate _next;


        public VisitorTrackingMiddleware(
            RequestDelegate next
            )
        {
            _next = next;

        }

        public async Task InvokeAsync
            (HttpContext context ,
            ITenantProvider _tenantProvider , 
            IVisitorSessionRepository _sessionRepository,
            IWebsiteAnalyticsRepository _analyticsRepository)
        {
            var path = context.Request.Path;

            if (!(path.StartsWithSegments("/api/website/storefront") ||
                  path.StartsWithSegments("/api/website/cart") ||
                  path.StartsWithSegments("/api/orders")))
            {
                await _next(context);
                return;
            }
            var tenantId = _tenantProvider.GetTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                await _next(context);
                return;
            }

            string sessionId;

            if (context.Request.Cookies.TryGetValue(".VisitorSessionId", out var cookieSessionId))
            {
                sessionId = cookieSessionId!;
            }
            else
            {
                sessionId = Guid.NewGuid().ToString("N");

                context.Response.Cookies.Append(
                    ".VisitorSessionId",
                    sessionId,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTime.UtcNow.AddDays(1)
                    });
            }



            string? userId = context.User.Identity?.IsAuthenticated == true
                ? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                : null;

  

            string? ipAddress =
                context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                ?? context.Connection.RemoteIpAddress?.ToString();


            string? userAgent = context.Request.Headers["User-Agent"].ToString();

 
            var existingSession = await _sessionRepository.GetBySessionIdAsync(sessionId);

            if (existingSession == null)
            {
                var newSession = new WebsiteVisitorSession
                {
                    SessionId = sessionId,
                    TenantId = tenantId,
                    UserId = userId,
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                    StartedAt = DateTime.UtcNow,
                    LastSeenAt = DateTime.UtcNow
                };

                await _sessionRepository.CreateAsync(newSession);

                await _analyticsRepository.IncrementVisitorsAsync();
            }
            else
            {
                if (existingSession.LastSeenAt < DateTime.UtcNow.AddMinutes(-1))
                {
                    await _sessionRepository.UpdateLastSeenAsync(existingSession);
                }
            }

            await _next(context);
        }
    }
}