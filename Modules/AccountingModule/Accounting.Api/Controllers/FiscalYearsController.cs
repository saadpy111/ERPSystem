using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Core.Constants.Permissions;
using Accounting.Application.Features.Fiscal.Commands.CreateFiscalYear;
using Accounting.Application.Features.Fiscal.Commands.UpdateFiscalYear;
using Accounting.Application.Features.Fiscal.Commands.CloseFiscalYear;
using Accounting.Application.Features.Fiscal.Commands.OpenFiscalYear;
using Accounting.Application.Features.Fiscal.Queries.GetFiscalYears;
using Accounting.Application.Features.Fiscal.Queries.GetFiscalYearById;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/fiscal/years")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Accounting")]
    public class FiscalYearsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FiscalYearsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [HasPermission(AccountingPermissions.FiscalView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetYears(CancellationToken cancellationToken)
        {
            var query = new GetFiscalYearsQuery();
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [HasPermission(AccountingPermissions.FiscalView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetYearById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var query = new GetFiscalYearByIdQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost]
        [HasPermission(AccountingPermissions.FiscalManage)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateYear([FromBody] CreateFiscalYearCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [HasPermission(AccountingPermissions.FiscalManage)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateYear([FromRoute] int id, [FromBody] UpdateFiscalYearCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("close")]
        [HasPermission(AccountingPermissions.FiscalManage)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CloseYear([FromBody] CloseFiscalYearCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("open")]
        [HasPermission(AccountingPermissions.FiscalManage)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> OpenYear([FromBody] OpenFiscalYearCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
