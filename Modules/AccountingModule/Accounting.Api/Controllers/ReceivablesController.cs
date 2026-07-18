using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Authorization;
using Accounting.Application.Features.Receivables.Commands;
using Accounting.Application.Features.Receivables.Queries.GetReceivablesPaged;
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
        public async Task<IActionResult> GetAll([FromQuery] GetReceivablesPagedQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [HasPermission(AccountingPermissions.ReceivablesCreate)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateReceivableCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            
            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
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
            
            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }
    }
}
