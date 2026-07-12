using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Multitenancy;
using Subscription.Application.Features.Subscriptions.Queries.GetSubscriptionDashboard;
using System.Threading.Tasks;

namespace Subscription.Api.Controllers
{
    [ApiController]
    [Route("api/subscription")]
    [ApiExplorerSettings(GroupName = "Subscription")]
    [Authorize]
    public class SubscriptionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITenantProvider _tenantProvider;

        public SubscriptionController(IMediator mediator, ITenantProvider tenantProvider)
        {
            _mediator = mediator;
            _tenantProvider = tenantProvider;
        }

        /// <summary>
        /// Retrieves the comprehensive subscription dashboard information for the current tenant.
        /// Includes plan, usage quotas, active modules, and purchased add-ons.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetSubscriptionDashboardResponse), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetDashboard()
        {
            var tenantId = _tenantProvider.GetTenantId();
            if (string.IsNullOrEmpty(tenantId))
            {
                return Unauthorized();
            }

            var query = new GetSubscriptionDashboardQuery { TenantId = tenantId };
            var result = await _mediator.Send(query);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
