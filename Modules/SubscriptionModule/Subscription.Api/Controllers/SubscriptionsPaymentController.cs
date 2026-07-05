using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Multitenancy;
using Subscription.Application.Features.SubscriptionRenewalPayment.Commands.CreateSubscriptionRenewalPayment;
using System.Security.Claims;

namespace Subscription.Api.Controllers
{
    [ApiController]
    [Route("api/subscriptions")]
    [ApiExplorerSettings(GroupName = "Subscription")]
    [Authorize]
    public sealed class SubscriptionsPaymentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITenantProvider _tenantProvider;

        public SubscriptionsPaymentController(IMediator mediator, ITenantProvider tenantProvider)
        {
            _mediator = mediator;
            _tenantProvider = tenantProvider;
        }

        [HttpPost("renew/payment")]
        public async Task<IActionResult> RenewSubscriptionPayment(
            [FromBody] SubscriptionRenewalPaymentRequest request)
        {
            var tenantId = _tenantProvider.GetTenantId();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _mediator.Send(new CreateSubscriptionRenewalPaymentCommand
            {
                TenantId = tenantId,
                UserId = userId,
                CustomerFirstName = request.CustomerFirstName,
                CustomerLastName = request.CustomerLastName,
                CustomerEmail = request.CustomerEmail,
                CustomerPhone = request.CustomerPhone
            });

            if (!result.Success)
                return BadRequest(new { Success = false, Error = result.Error });

            return Ok(result);
        }
    }

    public sealed class SubscriptionRenewalPaymentRequest
    {
        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
    }
}
