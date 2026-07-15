using MediatR;
using Microsoft.AspNetCore.Mvc;
using Website.Application.Features.WalletFeatures.Commands.RequestWithdrawal;
using Website.Application.Features.WalletFeatures.Queries.GetWallet;
using Website.Application.Features.WalletFeatures.Queries.GetWalletTransactions;

namespace Website.Api.Controllers
{
    [ApiController]
    [Route("api/wallet")]
    [ApiExplorerSettings(GroupName = "Website")]
    public class WalletController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WalletController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetWallet()
        {
            var query = new GetWalletQuery();
            var result = await _mediator.Send(query);

            if (!result.Success)
                return BadRequest(new { error = result.Error });

            return Ok(new
            {
                walletId = result.WalletId,
                currentBalance = result.CurrentBalance
            });
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = new GetWalletTransactionsQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);

            if (!result.Success)
                return BadRequest(new { error = result.Error });

            return Ok(result);
        }

        [HttpPost("withdrawals")]
        public async Task<IActionResult> RequestWithdrawal([FromBody] RequestWithdrawalRequest request)
        {
            var command = new RequestWithdrawalCommand
            {
                Amount = request.Amount,
                Notes = request.Notes
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(new { error = result.Error });

            return Ok(new { withdrawalRequestId = result.WithdrawalRequestId });
        }
    }

    public record RequestWithdrawalRequest(decimal Amount, string? Notes);
}
