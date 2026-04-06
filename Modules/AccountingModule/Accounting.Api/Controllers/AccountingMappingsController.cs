using Accounting.Application.Features.AccountingMappings.Commands.CreateAccountingMapping;
using Accounting.Application.Features.AccountingMappings.Commands.DeleteAccountingMapping;
using Accounting.Application.Features.AccountingMappings.Commands.UpdateAccountingMapping;
using Accounting.Application.Features.AccountingMappings.Queries.GetAccountingMappingById;
using Accounting.Application.Features.AccountingMappings.Queries.GetAccountingMappings;
using Accounting.Domain.Enums;
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
    [Route("api/accounting-mappings")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Accounting")]
    public class AccountingMappingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountingMappingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [HasPermission(AccountingPermissions.AccountingMappingsView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] SourceType? sourceType, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAccountingMappingsQuery { SourceType = sourceType }, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [HasPermission(AccountingPermissions.AccountingMappingsView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAccountingMappingByIdQuery { Id = id }, cancellationToken);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [HasPermission(AccountingPermissions.AccountingMappingsCreate)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateAccountingMappingCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        [HasPermission(AccountingPermissions.AccountingMappingsEdit)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAccountingMappingCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [HasPermission(AccountingPermissions.AccountingMappingsDelete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteAccountingMappingCommand { Id = id }, cancellationToken);
            return NoContent();
        }
    }
}
