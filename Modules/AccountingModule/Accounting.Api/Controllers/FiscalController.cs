using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Authorization;
using SharedKernel.Core.Constants.Permissions;

// Existing commands
using Accounting.Application.Features.Fiscal.Commands.CloseFiscalPeriod;
using Accounting.Application.Features.Fiscal.Commands.CreateFiscalYear;
using Accounting.Application.Features.Fiscal.Commands.OpenFiscalPeriod;

// New commands
using Accounting.Application.Features.Fiscal.Commands.UpdateFiscalYear;
using Accounting.Application.Features.Fiscal.Commands.CloseFiscalYear;
using Accounting.Application.Features.Fiscal.Commands.OpenFiscalYear;
using Accounting.Application.Features.Fiscal.Commands.CreateFiscalPeriod;
using Accounting.Application.Features.Fiscal.Commands.UpdateFiscalPeriod;
using Accounting.Application.Features.Fiscal.Commands.DeleteFiscalPeriod;

// Existing queries
using Accounting.Application.Features.Fiscal.Queries.GetFiscalPeriods;
using Accounting.Application.Features.Fiscal.Queries.GetFiscalYears;

// New queries
using Accounting.Application.Features.Fiscal.Queries.GetFiscalYearById;
using Accounting.Application.Features.Fiscal.Queries.GetFiscalPeriodById;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/fiscal")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Accounting")]
    public class FiscalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FiscalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // =====================================================================
        // FISCAL YEAR ENDPOINTS
        // =====================================================================

        /// <summary>Returns all fiscal years for the current tenant.</summary>
        [HttpGet("years")]
        [HasPermission(AccountingPermissions.FiscalView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetYears(CancellationToken cancellationToken)
        {
            var query = new GetFiscalYearsQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>Returns a single fiscal year by its ID.</summary>
        [HttpGet("years/{id:int}")]
        [HasPermission(AccountingPermissions.FiscalView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetYearById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var query = new GetFiscalYearByIdQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>Creates a new fiscal year and auto-generates 12 monthly periods.</summary>
        [HttpPost("years")]
        [HasPermission(AccountingPermissions.FiscalManage)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateYear([FromBody] CreateFiscalYearCommand command, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetYearById), new { id }, new { id });
        }

        /// <summary>Updates an existing open fiscal year.</summary>
        [HttpPut("years/{id:int}")]
        [HasPermission(AccountingPermissions.FiscalManage)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateYear([FromRoute] int id, [FromBody] UpdateFiscalYearCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        /// <summary>Closes a fiscal year. All periods must be closed first.</summary>
        [HttpPost("years/close")]
        [HasPermission(AccountingPermissions.FiscalManage)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CloseYear([FromBody] CloseFiscalYearCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        /// <summary>Re-opens a previously closed fiscal year.</summary>
        [HttpPost("years/open")]
        [HasPermission(AccountingPermissions.FiscalManage)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> OpenYear([FromBody] OpenFiscalYearCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        // =====================================================================
        // FISCAL PERIOD ENDPOINTS
        // =====================================================================

        /// <summary>Returns all fiscal periods, optionally filtered by fiscal year.</summary>
        [HttpGet("periods")]
        [HasPermission(AccountingPermissions.FiscalView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPeriods([FromQuery] int? fiscalYearId, CancellationToken cancellationToken)
        {
            var query = new GetFiscalPeriodsQuery { FiscalYearId = fiscalYearId };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>Returns a single fiscal period by its ID.</summary>
        [HttpGet("periods/{id:int}")]
        [HasPermission(AccountingPermissions.FiscalView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPeriodById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var query = new GetFiscalPeriodByIdQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>Manually creates a fiscal period within an existing open fiscal year.</summary>
        [HttpPost("periods")]
        [HasPermission(AccountingPermissions.FiscalManage)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePeriod([FromBody] CreateFiscalPeriodCommand command, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetPeriodById), new { id }, new { id });
        }

        /// <summary>Updates an existing open fiscal period.</summary>
        [HttpPut("periods/{id:int}")]
        [HasPermission(AccountingPermissions.FiscalManage)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePeriod([FromRoute] int id, [FromBody] UpdateFiscalPeriodCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        /// <summary>Closes a fiscal period. Closed periods cannot receive new journal entries.</summary>
        [HttpPost("periods/close")]
        [HasPermission(AccountingPermissions.FiscalManage)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ClosePeriod([FromBody] CloseFiscalPeriodCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        /// <summary>Re-opens a previously closed fiscal period.</summary>
        [HttpPost("periods/open")]
        [HasPermission(AccountingPermissions.FiscalManage)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> OpenPeriod([FromBody] OpenFiscalPeriodCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        /// <summary>Soft-deletes an open fiscal period.</summary>
        [HttpDelete("periods/{id:int}")]
        [HasPermission(AccountingPermissions.FiscalManage)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePeriod([FromRoute] int id, CancellationToken cancellationToken)
        {
            var command = new DeleteFiscalPeriodCommand { Id = id };
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }
    }
}
