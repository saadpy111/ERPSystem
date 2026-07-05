using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Subscription.Application.DTOs.PaymentDtos;
using Subscription.Application.Features.Payments.Commands.InitiatePayment;
using Subscription.Application.Features.Payments.Commands.ProcessWebhook;
using Subscription.Domain.Enums;

namespace Subscription.Api.Controllers
{
    [Route("api/payment")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Subscription")]
    public sealed class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IMediator mediator, ILogger<PaymentController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Initiate a Paymob payment intention for a given purpose (e.g. ModulePurchase, SubscriptionRenewal).
        /// </summary>
        //[Obsolete("Use business-specific payment endpoints: POST /api/companies/payment, POST /api/modules/payment, " +
        //          "POST /api/modules/renew/payment, POST /api/subscriptions/renew/payment")]
        //[HttpPost("pay")]
        //[ProducesResponseType(typeof(PayResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        //public async Task<IActionResult> Pay([FromBody] PayRequest request, CancellationToken cancellationToken)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return ValidationProblem(ModelState);
        //    }

        //    var command = new InitiatePaymentCommand
        //    {
               
        //        Purpose = request.Purpose,
        //        TargetId = request.TargetId,
        //        CurrencyCode = request.CurrencyCode,
        //        Interval = request.Interval,
        //        CustomerFirstName = request.CustomerFirstName,
        //        CustomerLastName = request.CustomerLastName,
        //        CustomerEmail = request.CustomerEmail,
        //        CustomerPhone = request.CustomerPhone
        //    };

        //    var result = await _mediator.Send(command, cancellationToken);

        //    if (!result.Success)
        //    {
        //        return Problem(
        //            title: "Payment initiation failed.",
        //            detail: result.Error,
        //            statusCode: StatusCodes.Status422UnprocessableEntity);
        //    }

        //    return Ok(new PayResponse(
        //        PaymentId: result.PaymentId ?? string.Empty,
        //        ClientSecret: result.ClientSecret ?? string.Empty,
        //        CheckoutUrl: result.CheckoutUrl ?? string.Empty,
        //        ReferenceId: result.ReferenceId ?? string.Empty,
        //        PublicKey: result.PublicKey ?? string.Empty,
        //        Status: result.Status ?? PaymentStatus.Pending.ToString(),
        //        IsReused: result.IsReused));
        //}

        /// <summary>
        /// Paymob webhook endpoint for transaction status callbacks. Validates HMAC and processes the event.
        /// </summary>
        [HttpPost("verify")]
        [ProducesResponseType(typeof(VerifyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Verify(
            [FromQuery] string hmac,
            [FromBody] WebhookDto callback,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(hmac))
            {
                return Problem(title: "Missing HMAC.", statusCode: StatusCodes.Status400BadRequest);
            }

            // Skip events that are not TRANSACTION type
            if (!string.Equals(callback.Type, "TRANSACTION", StringComparison.OrdinalIgnoreCase))
            {
                return Ok(new VerifyResponse(true, "Non-transaction event acknowledged."));
            }

            try
            {
                var command = new ProcessWebhookCommand(hmac, callback);
                var result = await _mediator.Send(command, cancellationToken);

                return Ok(new VerifyResponse(result.Success, result.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception processing Paymob webhook.");
                // Return 500 so Paymob retries the webhook
                return StatusCode(StatusCodes.Status500InternalServerError, new VerifyResponse(false, "Internal error processing webhook."));
            }
        }
    }
}
