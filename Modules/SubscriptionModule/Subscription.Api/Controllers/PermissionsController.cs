using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;
using SharedKernel.Multitenancy;
using Subscription.Application.Features.Permissions.Commands.SyncTenantPermissions;

namespace Subscription.Api.Controllers
{
    [ApiController]
    [Route("api/subscription/permissions")]
    [ApiExplorerSettings(GroupName = "Subscription")]
    [Authorize]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITenantProvider _tenantProvider;

        public PermissionsController(IMediator mediator, ITenantProvider tenantProvider)
        {
            _mediator = mediator;
            _tenantProvider = tenantProvider;
        }

        [HttpPost("sync")]
        [HasPermission(AdminPermissions.RolesAssignPermissions)]
        public async Task<IActionResult> SyncTenantPermissions()
        {
            var tenantId = _tenantProvider.GetTenantId();
            if (string.IsNullOrEmpty(tenantId))
                return Unauthorized();

            var result = await _mediator.Send(new SyncTenantPermissionsCommand
            {
                TenantId = tenantId
            });

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
