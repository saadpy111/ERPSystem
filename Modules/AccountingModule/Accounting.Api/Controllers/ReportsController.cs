using Accounting.Application.Reports.DTOs;
using Accounting.Application.Reports.Queries.GetBalanceSheet;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/reports")]
    [Produces("application/json")]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves the Balance Sheet report as of a specified date.
        /// </summary>
        /// <param name="asOfDate">The cutoff date for the report.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A Balance Sheet divided into Assets, Liabilities, and Equity</returns>
        [HttpGet("balance-sheet")]
        [ProducesResponseType(typeof(BalanceSheetDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBalanceSheet(
            [FromQuery] DateTime asOfDate,
            CancellationToken cancellationToken)
        {
            var query = new GetBalanceSheetQuery { AsOfDate = asOfDate };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
