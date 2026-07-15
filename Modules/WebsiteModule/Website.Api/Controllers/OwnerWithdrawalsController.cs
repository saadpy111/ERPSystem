using MediatR;
using Microsoft.AspNetCore.Mvc;
using Website.Application.Features.WalletFeatures.Commands.ApproveWithdrawal;
using Website.Application.Features.WalletFeatures.Commands.RejectWithdrawal;
using Website.Application.Features.WalletFeatures.Queries.GetWithdrawalRequests;

namespace Website.Api.Controllers
{
    [ApiController]
    [Route("api/owner/withdrawals")]
    [ApiExplorerSettings(GroupName = "Website")]
    public class OwnerWithdrawalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OwnerWithdrawalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetWithdrawalRequests(
            [FromQuery] string? statusFilter,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = new GetWithdrawalRequestsQuery
            {
                StatusFilter = statusFilter,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);

            if (!result.Success)
                return BadRequest(new { error = result.Error });

            return Ok(result);
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> Approve(Guid id)
        {
            var command = new ApproveWithdrawalCommand
            {
                WithdrawalRequestId = id,
                ReviewedBy = "saas-admin"
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(new { error = result.Error });

            return Ok(new { message = "Withdrawal approved." });
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(Guid id, [FromBody] RejectWithdrawalRequest? body)
        {
            var command = new RejectWithdrawalCommand
            {
                WithdrawalRequestId = id,
                ReviewedBy = "saas-admin",
                Notes = body?.Notes
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(new { error = result.Error });

            return Ok(new { message = "Withdrawal rejected." });
        }
    }

    public record RejectWithdrawalRequest(string? Notes);
}
