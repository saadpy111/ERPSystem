using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;
using Accounting.Application.Features.Accounts.Commands.CreateAccount;
using Accounting.Application.Features.Accounts.Commands.UpdateAccount;
using Accounting.Application.Features.Accounts.Queries.GetAccountById;
using Accounting.Application.Features.Accounts.Queries.GetAccountHierarchy;
using Accounting.Application.Features.Accounts.Queries.GetAccountsList;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/accounts")]
    [Produces("application/json")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [HasPermission(AccountingPermissions.AccountsView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAccountsListQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("tree")]
        [HasPermission(AccountingPermissions.AccountsView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTree(CancellationToken cancellationToken)
        {
            var query = new GetAccountHierarchyQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [HasPermission(AccountingPermissions.AccountsView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var query = new GetAccountByIdQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [HasPermission(AccountingPermissions.AccountsCreate)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAccountCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result }, result);
        }

        [HttpPut("{id:int}")]
        [HasPermission(AccountingPermissions.AccountsEdit)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateAccountCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in route must match ID in body.");
            }

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
