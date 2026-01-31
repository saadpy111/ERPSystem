using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website.Application.Features.TenantWebsite.Commands.ApplyTheme;
using Website.Application.Features.TenantWebsite.Commands.UpdateConfig;
using Website.Application.Features.TenantWebsite.Queries.GetTenantWebsiteConfig;
using System.Security.Claims;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;

namespace Website.Api.Controllers
{
    /// <summary>
    /// Tenant website configuration API.
    /// </summary>
    [ApiController]
    [Route("api/website/config")]
    [ApiExplorerSettings(GroupName = "Website")]
    [Authorize]
    public class WebsiteConfigController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WebsiteConfigController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private string GetTenantId()
        {
            return User.FindFirst("tenant")?.Value ?? string.Empty;
        }

        /// <summary>
        /// Get current tenant website configuration
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetTenantWebsiteConfigResponse), 200)]
        [ProducesResponseType(404)]
        [HasPermission(WebsitePermissions.ConfigView)]
        public async Task<IActionResult> GetConfig()
        {
            var tenantId = GetTenantId();
            
            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(new { error = "Tenant context required" });

            var result = await _mediator.Send(new GetTenantWebsiteConfigQuery { TenantId = tenantId });
            
            if (!result.Success)
                return NotFound(new { error = result.Error });
            
            return Ok(result);
        }

        /// <summary>
        /// Update tenant website configuration
        /// </summary>
        [HttpPut]
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
