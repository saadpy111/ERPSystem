using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using Subscription.Application.Features.CompanyPayment.Commands.CreateCompanyPayment;
using System.Security.Claims;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("api/companies")]
    [Authorize]
    [ApiExplorerSettings(GroupName = "Identity")]
    public sealed class CompaniesPaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompaniesPaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("payment")]
        public async Task<IActionResult> CreateCompanyPayment(
            [FromBody] CreateCompanyPaymentRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            if (!Enum.TryParse<BillingInterval>(request.BillingInterval, true, out var interval))
                return BadRequest(new { Success = false, Error = "Invalid billing interval. Use Monthly, Quarterly, or Yearly." });

            var command = new CreateCompanyPaymentCommand
            {
                UserId = userId,
                CompanyName = request.CompanyName,
                CompanyCode = request.CompanyCode,
                PlanCode = request.PlanCode,
                CurrencyCode = request.CurrencyCode,
                Interval = interval,
                CustomerFirstName = request.CustomerFirstName,
                CustomerLastName = request.CustomerLastName,
                CustomerEmail = request.CustomerEmail,
                CustomerPhone = request.CustomerPhone
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(new { Success = false, Error = result.Error });

            return Ok(result);
        }
    }

    public sealed class CreateCompanyPaymentRequest
    {
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyCode { get; set; } = string.Empty;
        public string PlanCode { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public string BillingInterval { get; set; } = "Monthly";
        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
    }
}
