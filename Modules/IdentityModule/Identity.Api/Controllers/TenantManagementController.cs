using Identity.Application.Features.TenantFeature.Commands.CreateCompany;
using Identity.Application.Features.TenantFeature.Commands.SendInvitation;
using Identity.Application.Features.TenantFeature.Commands.LeaveCompany;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("api/tenants")]
    [Authorize]
    [ApiExplorerSettings(GroupName = "Identity")]

    public class TenantManagementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TenantManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a new company (tenant). User must be in PendingTenant state.
        /// </summary>
        /// 
        [HttpPost("create")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateCompany([FromForm] CreateCompanyCommand command)
        {
            command.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var result = await _mediator.Send(command);
            
            if (!result.Success)
                return BadRequest(new { error = result.Error });
            
            return Ok(result);
        }

        /// <summary>
        /// Invite a user to join your company
        /// </summary>
        [HttpPost("invite")]
        public async Task<IActionResult> InviteUser([FromBody] SendInvitationCommand command)
        {
            command.InvitedByUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            command.TenantId = User.FindFirst("tenant")?.Value ?? string.Empty;
            
            var result = await _mediator.Send(command);
            
            if (!result.Success)
                return BadRequest(new { error = result.Error });
            
            return Ok(result);
        }

        /// <summary>
        /// Leave current company (members only, not owners)
        /// </summary>
        [HttpPost("leave")]
        public async Task<IActionResult> LeaveCompany()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var result = await _mediator.Send(new LeaveCompanyCommand { UserId = userId });
            
            if (!result.Success)
                return BadRequest(new { error = result.Error });
            
            return Ok(result);
        }
    }
}
