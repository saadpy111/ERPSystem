using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Website.Application.Features.WalletFeatures.Commands.ApproveWithdrawal;
using Website.Application.Features.WalletFeatures.Commands.RejectWithdrawal;
using Website.Application.Features.WalletFeatures.Queries.GetWithdrawalRequests;

namespace Website.Api.Controllers
{
    [ApiController]
    [Route("api/admin/withdrawals")]
    [ApiExplorerSettings(GroupName = "Website")]
    [Authorize]
    public class AdminWithdrawalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminWithdrawalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

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
                ReviewedBy = GetUserId()
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
                ReviewedBy = GetUserId(),
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
