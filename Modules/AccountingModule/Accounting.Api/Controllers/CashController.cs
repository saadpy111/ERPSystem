using System.Threading.Tasks;
using Accounting.Application.Features.Cash.Commands.CreateCashTransaction;
using Accounting.Application.Features.Cash.Queries.GetCashTransactions;
using Accounting.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Core.Constants.Permissions;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/cash")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Accounting")]

    public class CashController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CashController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [HasPermission(AccountingPermissions.CashView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] int? accountId)
        {
            var query = new GetCashTransactionsQuery { CashAccountId = accountId };
            var result = await _mediator.Send(query);
            
            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }

        [HttpPost("receipt")]
        [HasPermission(AccountingPermissions.CashCreate)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateReceipt([FromBody] CreateCashTransactionCommand command)
        {
            var result = await _mediator.Send(command);
            
            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }

        [HttpPost("payment")]
        [HasPermission(AccountingPermissions.CashCreate)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePayment([FromBody] CreateCashTransactionCommand command)
        {
            command.Type = CashTransactionType.Payment;
            var result = await _mediator.Send(command);
            
            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }
    }
}
