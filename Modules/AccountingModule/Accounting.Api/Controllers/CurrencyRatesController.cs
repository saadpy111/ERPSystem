using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Features.CurrencyRates.Commands.CreateCurrencyRate;
using Accounting.Application.Features.CurrencyRates.Queries.GetCurrencyRatesList;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Core.Constants.Permissions;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/currency-rates")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Accounting")]
    public class CurrencyRatesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CurrencyRatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [HasPermission(AccountingPermissions.CurrencyRatesView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] int currencyId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCurrencyRatesQuery { CurrencyId = currencyId }, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [HasPermission(AccountingPermissions.CurrencyRatesCreate)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateCurrencyRateCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
