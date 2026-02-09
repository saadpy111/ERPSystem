using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;
using SharedKernel.Multitenancy;
using SharedKernel.Website;
using System.Security.Claims;
using Website.Application.Features.TenantWebsite.Commands.ApplyTheme;
using Website.Application.Features.TenantWebsite.Commands.UpdateConfig;
using Website.Application.Features.TenantWebsite.Queries.GetTenantWebsiteConfig;

namespace Website.Api.Controllers
{
    /// <summary>
    /// Tenant website configuration API.
    /// </summary>
    [ApiController]
    [Route("api/website/config")]
    [ApiExplorerSettings(GroupName = "Website")]
   // [Authorize]
    public class WebsiteConfigController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITenantProvider _tenantProvider;

        public WebsiteConfigController(IMediator mediator , ITenantProvider tenantProvider)
        {
            _mediator = mediator;
            _tenantProvider = tenantProvider;
        }

        private string? GetTenantId()
        {
            return _tenantProvider.GetTenantId();
        }

        /// <summary>
        /// Get current tenant website configuration
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetTenantWebsiteConfigResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetConfig(
            [FromServices] ITenantProvider tenantProvider,
            [FromServices] ITenantDomainResolver tenantDomainResolver)
        {
            if (!Request.Headers.TryGetValue("X-Tenant-Key", out var tenantKey))
            {
                return BadRequest(new { error = "X-Tenant-Key header is required" });
            }

            var tenantResult = await tenantDomainResolver.GetTenantByDomainAsync(tenantKey!);

            if (tenantResult == null)
            {
                return NotFound(new { error = "Tenant not found for provided domain" });
            }

            tenantProvider.SetTenantId(tenantResult.TenantId);

            var tenantId = tenantResult.TenantId;

            var result = await _mediator.Send(
                new GetTenantWebsiteConfigQuery { TenantId = tenantId });

            if (!result.Success)
                return NotFound(new { error = result.Error });

            return Ok(result);
        }

        /// <summary>
        /// Update tenant website configuration
        /// </summary>
        [HttpPut]
        [Authorize]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(UpdateTenantWebsiteConfigResponse), 200)]
        [ProducesResponseType(400)]
        [HasPermission(WebsitePermissions.ConfigEdit)]

        public async Task<IActionResult> UpdateConfig([FromForm] UpdateTenantWebsiteConfigCommand command)
        {
            var tenantId = GetTenantId();
            
            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(new { error = "Tenant context required" });

            command.TenantId = tenantId;
            var result = await _mediator.Send(command);
            
            if (!result.Success)
                return BadRequest(new { error = result.Error });
            
            return Ok(result);
        }

        /// <summary>
        /// Apply a theme to tenant website
        /// </summary>
        [HttpPost("apply-theme")]
        [Authorize]
        [ProducesResponseType(typeof(ApplyThemeResponse), 200)]
        [ProducesResponseType(400)]
        [HasPermission(WebsitePermissions.ConfigApplyTheme)]
        public async Task<IActionResult> ApplyTheme([FromBody] ApplyThemeRequest request)
        {
            var tenantId = GetTenantId();
            
            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(new { error = "Tenant context required" });

            var command = new ApplyThemeCommand
            {
                TenantId = tenantId,
                ThemeId = request.ThemeId
            };

            var result = await _mediator.Send(command);
            
            if (!result.Success)
                return BadRequest(new { error = result.Error });
            
            return Ok(result);
        }
    }

    public class ApplyThemeRequest
    {
        public Guid ThemeId { get; set; }
    }
}
