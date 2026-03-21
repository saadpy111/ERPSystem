using Identity.Application.Features.AuthFeature.Queries.GetMyPermissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.Api.Controllers
{
    /// <summary>
    /// Endpoints scoped to the currently authenticated user ("me" pattern).
    /// </summary>
    [ApiController]
    [Route("api/me")]
    [Authorize]
    [ApiExplorerSettings(GroupName = "Identity")]
    public class MeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns all effective permissions (direct + role-based) for the
        /// authenticated user within their tenant.
        ///
        /// Always queries the database — not the JWT — so the frontend receives
        /// up-to-date data for UI visibility control (show/hide pages, buttons, etc.).
        /// </summary>
        /// <response code="200">List of permission names.</response>
        /// <response code="401">Missing or invalid JWT, or missing userId / tenantId claims.</response>
        [HttpGet("permissions")]
        [ProducesResponseType(typeof(GetMyPermissionsResponse), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetMyPermissions()
        {
            // Extract userId — support both "sub" (OIDC standard) and NameIdentifier (.NET default)
            var userId =
                User.FindFirstValue("sub") ??
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Extract tenantId from custom "tenant" claim
            var tenantId = User.FindFirstValue("tenant");

            // Fail-fast: both claims are required for a safe, tenant-isolated DB query
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(tenantId))
                return Unauthorized(new { error = "User identity or tenant context is missing from the token." });

            var query = new GetMyPermissionsQuery
            {
                UserId = userId,
                TenantId = tenantId
            };

            var response = await _mediator.Send(query);

            return Ok(new { permissions = response.Permissions });
        }
    }
}
