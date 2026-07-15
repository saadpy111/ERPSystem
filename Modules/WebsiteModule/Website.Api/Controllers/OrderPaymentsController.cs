using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Website.Application.Features.OrderPaymentFeatures.Commands.InitiateOrderPayment;
using Website.Application.Features.OrderPaymentFeatures.Queries.GetOrderPaymentStatus;

namespace Website.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [ApiExplorerSettings(GroupName = "Website")]
    [Authorize]
    public class OrderPaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderPaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        [HttpPost("{orderId}/pay")]
        public async Task<IActionResult> Pay(Guid orderId)
        {
            var command = new InitiateOrderPaymentCommand
            {
                OrderId = orderId,
                UserId = GetUserId()
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(new { error = result.Error });

            return Ok(new
            {
                result.PaymentId,
                result.CheckoutUrl,
                result.ClientSecret,
                result.PublicKey,
                result.IsReused
            });
        }

        [HttpGet("{orderId}/payment-status")]
        public async Task<IActionResult> GetPaymentStatus(Guid orderId)
        {
            var query = new GetOrderPaymentStatusQuery
            {
                OrderId = orderId,
                UserId = GetUserId()
            };

            var result = await _mediator.Send(query);

            if (!result.Success)
                return BadRequest(new { error = result.Error });

            return Ok(new
            {
                result.OrderStatus,
                result.PaymentStatus,
                result.IsPaid
            });
        }
    }
}
