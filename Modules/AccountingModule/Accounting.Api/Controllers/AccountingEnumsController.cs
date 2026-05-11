using Accounting.Application.Features.Lookups.Queries.GetEnumValues;
using Accounting.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/enums")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Accounting")]
    public class AccountingEnumsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountingEnumsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("voucher-types")]
        public async Task<IActionResult> GetVoucherTypes(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEnumValuesQuery(typeof(VoucherType)), cancellationToken);
            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("voucher-statuses")]
        public async Task<IActionResult> GetVoucherStatuses(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEnumValuesQuery(typeof(VoucherStatus)), cancellationToken);
            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("source-types")]
        public async Task<IActionResult> GetSourceTypes(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEnumValuesQuery(typeof(SourceType)), cancellationToken);
            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("receivable-statuses")]
        public async Task<IActionResult> GetReceivableStatuses(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEnumValuesQuery(typeof(ReceivableStatus)), cancellationToken);
            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("payment-methods")]
        public async Task<IActionResult> GetPaymentMethods(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEnumValuesQuery(typeof(PaymentMethod)), cancellationToken);
            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("payable-statuses")]
        public async Task<IActionResult> GetPayableStatuses(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEnumValuesQuery(typeof(PayableStatus)), cancellationToken);
            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("partner-types")]
        public async Task<IActionResult> GetPartnerTypes(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEnumValuesQuery(typeof(PartnerType)), cancellationToken);
            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("journal-statuses")]
        public async Task<IActionResult> GetJournalStatuses(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEnumValuesQuery(typeof(JournalStatus)), cancellationToken);
            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("cash-transaction-types")]
        public async Task<IActionResult> GetCashTransactionTypes(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEnumValuesQuery(typeof(CashTransactionType)), cancellationToken);
            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("budget-statuses")]
        public async Task<IActionResult> GetBudgetStatuses(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEnumValuesQuery(typeof(BudgetStatus)), cancellationToken);
            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("account-types")]
        public async Task<IActionResult> GetAccountTypes(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEnumValuesQuery(typeof(AccountType)), cancellationToken);
            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("report-formats")]
        public async Task<IActionResult> GetReportFormats(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEnumValuesQuery(typeof(Accounting.Application.Common.Enums.ReportFormat)), cancellationToken);
            return Ok(new { success = true, data = result.Data });
        }
    }
}
