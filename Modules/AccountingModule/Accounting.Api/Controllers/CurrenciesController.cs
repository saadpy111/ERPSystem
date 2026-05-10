using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Features.Currencies.Commands.CreateCurrency;
using Accounting.Application.Features.Currencies.Commands.SetBaseCurrency;
using Accounting.Application.Features.Currencies.Commands.UpdateCurrency;
using Accounting.Application.Features.Currencies.Queries.GetCurrenciesList;
using Accounting.Application.Features.Currencies.Queries.GetCurrencyById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Core.Constants.Permissions;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/currencies")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Accounting")]
    public class CurrenciesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CurrenciesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [HasPermission(AccountingPermissions.CurrenciesView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCurrenciesQuery(), cancellationToken);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [HasPermission(AccountingPermissions.CurrenciesView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCurrencyByIdQuery { Id = id }, cancellationToken);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [HasPermission(AccountingPermissions.CurrenciesCreate)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateCurrencyCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [HasPermission(AccountingPermissions.CurrenciesEdit)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCurrencyCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id:int}/set-base")]
        [HasPermission(AccountingPermissions.CurrenciesEdit)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SetBase(int id, [FromBody] SetBaseCurrencyCommand command, CancellationToken cancellationToken)
        {
            command.CurrencyId = id;
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
