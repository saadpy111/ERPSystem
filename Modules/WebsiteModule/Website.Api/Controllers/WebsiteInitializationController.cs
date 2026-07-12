using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;
using SharedKernel.Multitenancy;
using Website.Application.Features.WebsiteInitialization.Commands.InitializeWebsite;
using Website.Application.Features.WebsiteInitialization.Queries.CheckDomainAvailability;

namespace Website.Api.Controllers
{
    [ApiController]
    [Route("api/websites")]
    [Authorize]
    [ApiExplorerSettings(GroupName = "Website")]
    public class WebsiteInitializationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITenantProvider _tenantProvider;

        public WebsiteInitializationController(IMediator mediator, ITenantProvider tenantProvider)
        {
            _mediator = mediator;
            _tenantProvider = tenantProvider;
        }

        [HttpPost("initialize")]
        [Consumes("multipart/form-data")]
        [HasPermission(WebsitePermissions.WebsiteBuilder)]
        public async Task<IActionResult> InitializeWebsite([FromForm] InitializeWebsiteCommand command)
        {
            var tenantId = _tenantProvider.GetTenantId();
            if (string.IsNullOrEmpty(tenantId))
                return BadRequest(new { error = "Tenant context required." });

            command.TenantId = tenantId;
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(new { error = result.Error });

            return Ok(new { websiteId = result.WebsiteId });
        }

        [HttpGet("check-domain")]
        [HasPermission(WebsitePermissions.WebsiteBuilder)]
        public async Task<IActionResult> CheckDomainAvailability([FromQuery] string domain="")
        {
            var result = await _mediator.Send(new CheckDomainAvailabilityQuery { Domain = domain });

            if (!result.Success)
                return BadRequest(new { error = result.Error });

            return Ok(new { isAvailable = result.IsAvailable });
        }
    }
}
