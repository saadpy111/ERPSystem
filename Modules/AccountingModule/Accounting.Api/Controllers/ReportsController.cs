using Accounting.Application.Reports.DTOs;
using Accounting.Application.Reports.Queries.GetAccountStatement;
using Accounting.Application.Reports.Queries.GetBalanceSheet;
using Accounting.Application.Reports.Queries.GetGeneralLedger;
using Accounting.Application.Reports.Queries.GetIncomeStatement;
using Accounting.Application.Reports.Queries.GetTrialBalance;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        [HttpGet("trial-balance")]
        [ProducesResponseType(typeof(IEnumerable<TrialBalanceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTrialBalance(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            CancellationToken cancellationToken)
        {
            var query = new GetTrialBalanceQuery { FromDate = fromDate, ToDate = toDate };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("general-ledger")]
        [ProducesResponseType(typeof(IEnumerable<LedgerDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGeneralLedger(
            [FromQuery] int accountId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            CancellationToken cancellationToken)
        {
            var query = new GetGeneralLedgerQuery { AccountId = accountId, FromDate = fromDate, ToDate = toDate };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("account-statement")]
        [ProducesResponseType(typeof(AccountStatementDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccountStatement(
            [FromQuery] int partnerId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            CancellationToken cancellationToken)
        {
            var query = new GetAccountStatementQuery { PartnerId = partnerId, FromDate = fromDate, ToDate = toDate };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
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

        /// <summary>
        /// Retrieves the Income Statement (Profit & Loss) report for a specified date range.
        /// </summary>
        /// <param name="fromDate">The start date for the report.</param>
        /// <param name="toDate">The end date for the report.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An Income Statement divided into Revenues and Expenses with Net Profit</returns>
        [HttpGet("income-statement")]
        [ProducesResponseType(typeof(IncomeStatementDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetIncomeStatement(
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate,
            CancellationToken cancellationToken)
        {
            var query = new GetIncomeStatementQuery { FromDate = fromDate, ToDate = toDate };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
