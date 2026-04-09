using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Authorization;
using Accounting.Application.Features.Receivables.Commands;
using SharedKernel.Core.Constants.Permissions;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/receivables")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Accounting")]
    public class ReceivablesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReceivablesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [HasPermission(AccountingPermissions.ReceivablesView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            // Example of MediatR query call placeholder
            // var query = new GetReceivablesListQuery();
            // var result = await _mediator.Send(query, cancellationToken);
            // return Ok(result);
            return Ok(new { Message = "GET /receivables - Query implementation goes here" });
        }

        [HttpPost]
        [HasPermission(AccountingPermissions.ReceivablesCreate)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateReceivableCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetAll), new { id = result }, result);
        }

        [HttpPost("{id:int}/pay")]
        [HasPermission(AccountingPermissions.ReceivablesPay)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Pay([FromRoute] int id, [FromBody] ReceivePaymentCommand command, CancellationToken cancellationToken)
        {
            if (id != command.ReceivableId)
            {
                return BadRequest("ID in route must match ReceivableId in body.");
            }

            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new { PaymentId = result });
        }
    }
}
