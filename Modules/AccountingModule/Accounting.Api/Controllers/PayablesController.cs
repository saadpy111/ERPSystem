using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Authorization;
using Accounting.Application.Features.Payables.Commands;
using Accounting.Application.Features.Payables.Queries.GetPayablesPaged;
using SharedKernel.Core.Constants.Permissions;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/payables")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Accounting")]
    public class PayablesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PayablesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [HasPermission(AccountingPermissions.PayablesView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetPayablesPagedQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [HasPermission(AccountingPermissions.PayablesCreate)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreatePayableCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            
            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }

        [HttpPost("{id:int}/pay")]
        [HasPermission(AccountingPermissions.PayablesPay)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Pay([FromRoute] int id, [FromBody] PayPayableCommand command, CancellationToken cancellationToken)
        {
            if (id != command.PayableId)
            {
                return BadRequest("ID in route must match PayableId in body.");
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
