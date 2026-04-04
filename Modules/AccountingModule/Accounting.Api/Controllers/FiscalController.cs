using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Features.Fiscal.Commands.CloseFiscalPeriod;
using Accounting.Application.Features.Fiscal.Commands.CreateFiscalYear;
using Accounting.Application.Features.Fiscal.Commands.OpenFiscalPeriod;
using Accounting.Application.Features.Fiscal.Queries.GetFiscalPeriods;
using Accounting.Application.Features.Fiscal.Queries.GetFiscalYears;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/fiscal")]
    [Produces("application/json")]
    public class FiscalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FiscalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("years")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetYears(CancellationToken cancellationToken)
        {
            var query = new GetFiscalYearsQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("years")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateYear([FromBody] CreateFiscalYearCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpGet("periods")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPeriods([FromQuery] int? fiscalYearId, CancellationToken cancellationToken)
        {
            var query = new GetFiscalPeriodsQuery { FiscalYearId = fiscalYearId };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("periods/close")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ClosePeriod([FromBody] CloseFiscalPeriodCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPost("periods/open")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> OpenPeriod([FromBody] OpenFiscalPeriodCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
