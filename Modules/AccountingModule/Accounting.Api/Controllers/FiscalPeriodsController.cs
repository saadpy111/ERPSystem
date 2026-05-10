using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Core.Constants.Permissions;
using Accounting.Application.Features.Fiscal.Commands.CloseFiscalPeriod;
using Accounting.Application.Features.Fiscal.Commands.OpenFiscalPeriod;
using Accounting.Application.Features.Fiscal.Commands.CreateFiscalPeriod;
using Accounting.Application.Features.Fiscal.Commands.UpdateFiscalPeriod;
using Accounting.Application.Features.Fiscal.Commands.DeleteFiscalPeriod;
using Accounting.Application.Features.Fiscal.Queries.GetFiscalPeriods;
using Accounting.Application.Features.Fiscal.Queries.GetFiscalPeriodById;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/fiscal/periods")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Accounting")]
    public class FiscalPeriodsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FiscalPeriodsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [HasPermission(AccountingPermissions.FiscalView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPeriods([FromQuery] int? fiscalYearId, CancellationToken cancellationToken)
        {
            var query = new GetFiscalPeriodsQuery { FiscalYearId = fiscalYearId };
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
                return BadRequest(new { result.Message });

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }

        [HttpGet("{id:int}")]
        [HasPermission(AccountingPermissions.FiscalView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPeriodById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var query = new GetFiscalPeriodByIdQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
                return BadRequest(new { result.Message });

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }

        [HttpPost]
        [HasPermission(AccountingPermissions.FiscalManage)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePeriod([FromBody] CreateFiscalPeriodCommand command, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(command, cancellationToken);
            return Ok(new { id });
        }

        [HttpPut("{id:int}")]
        [HasPermission(AccountingPermissions.FiscalManage)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePeriod([FromRoute] int id, [FromBody] UpdateFiscalPeriodCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPost("close")]
        [HasPermission(AccountingPermissions.FiscalManage)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ClosePeriod([FromBody] CloseFiscalPeriodCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.Success)
                return BadRequest(new { result.Message });

            return Ok(new { success = true, message = result.Message });
        }

        [HttpPost("open")]
        [HasPermission(AccountingPermissions.FiscalManage)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> OpenPeriod([FromBody] OpenFiscalPeriodCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.Success)
                return BadRequest(new { result.Message });

            return Ok(new { success = true, message = result.Message });
        }

        [HttpDelete("{id:int}")]
        [HasPermission(AccountingPermissions.FiscalManage)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePeriod([FromRoute] int id, CancellationToken cancellationToken)
        {
            var command = new DeleteFiscalPeriodCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
