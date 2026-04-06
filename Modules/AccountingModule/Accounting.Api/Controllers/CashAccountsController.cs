using Accounting.Application.Features.CashAccounts.Commands.CreateCashAccount;
using Accounting.Application.Features.CashAccounts.Commands.DeleteCashAccount;
using Accounting.Application.Features.CashAccounts.Commands.UpdateCashAccount;
using Accounting.Application.Features.CashAccounts.Queries.GetCashAccountById;
using Accounting.Application.Features.CashAccounts.Queries.GetCashAccounts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Core.Constants.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/cash-accounts")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Accounting")]
    public class CashAccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CashAccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [HasPermission(AccountingPermissions.CashAccountsView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCashAccountsQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [HasPermission(AccountingPermissions.CashAccountsView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCashAccountByIdQuery { Id = id }, cancellationToken);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [HasPermission(AccountingPermissions.CashAccountsCreate)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateCashAccountCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        [HasPermission(AccountingPermissions.CashAccountsEdit)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCashAccountCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [HasPermission(AccountingPermissions.CashAccountsDelete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteCashAccountCommand { Id = id }, cancellationToken);
            return NoContent();
        }
    }
}
