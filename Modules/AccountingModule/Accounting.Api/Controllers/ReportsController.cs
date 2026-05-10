using Accounting.Application.Common.Enums;
using Accounting.Application.Interfaces.Services;
using Accounting.Application.Reports.DTOs;
using Accounting.Application.Reports.Queries;
using Accounting.Application.Reports.Queries.GetAccountStatement;
using Accounting.Application.Reports.Queries.GetBalanceSheet;
using Accounting.Application.Reports.Queries.GetGeneralLedger;
using Accounting.Application.Reports.Queries.GetIncomeStatement;
using Accounting.Application.Reports.Queries.GetTrialBalance;
using Accounting.Application.Reports.Builders;
using Accounting.Application.Reports.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Authorization;
using SharedKernel.Core.Constants.Permissions;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/reports")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Accounting")]

    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IReportExportService _exportService;

        public ReportsController(IMediator mediator, IReportExportService exportService)
        {
            _mediator = mediator;
            _exportService = exportService;
        }

        
        [HttpGet("trial-balance")]
        [HasPermission(AccountingPermissions.ReportsTrialBalance)]
        [ProducesResponseType(typeof(IEnumerable<TrialBalanceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTrialBalance(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] ReportFormat? format,
            CancellationToken cancellationToken)
        {
            var query = new GetTrialBalanceQuery { FromDate = fromDate, ToDate = toDate };
            var result = await _mediator.Send(query, cancellationToken);
            
            if (!result.Success) return BadRequest(new { result.Message });

            if (format.HasValue)
            {
                var reportName = "Trial Balance";
                var definition = new ReportDefinitionBuilder<TrialBalanceDto>()
                    .AddTextColumn(x => x.AccountCode, "Account Code")
                    .AddTextColumn(x => x.AccountName, "Account Name")
                    .AddDecimalColumn(x => x.Debit, "Turnover Debit")
                    .AddDecimalColumn(x => x.Credit, "Turnover Credit")
                    .AddDebitColumn(x => x.Balance, "Debit Balance", x => x.AccountType)
                    .AddCreditColumn(x => x.Balance, "Credit Balance", x => x.AccountType)
                    .HasTotals(true)
                    .Build();

                var (stream, contentType, fileName) = format.Value switch
                {
                    ReportFormat.Pdf => await _exportService.ExportToPdf(result.Data, definition),
                    ReportFormat.Excel => await _exportService.ExportToExcel(result.Data, definition),
                    ReportFormat.Csv => await _exportService.ExportToCsv(result.Data, definition),
                    _ => throw new ArgumentOutOfRangeException()
                };
                return File(stream, contentType, fileName);
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }

        [HttpGet("general-ledger")]
        [HasPermission(AccountingPermissions.ReportsGeneralLedger)]
        [ProducesResponseType(typeof(IEnumerable<LedgerDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGeneralLedger(
            [FromQuery] int accountId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] ReportFormat? format,
            CancellationToken cancellationToken)
        {
            var query = new GetGeneralLedgerQuery { AccountId = accountId, FromDate = fromDate, ToDate = toDate };
            var result = await _mediator.Send(query, cancellationToken);
            
            if (!result.Success) return BadRequest(new { result.Message });

            if (format.HasValue)
            {
                var reportName = "General Ledger";
                var definition = new ReportDefinitionBuilder<LedgerDto>()
                    .AddTextColumn(x => x.Date.ToString("yyyy-MM-dd"), "Date")
                    .AddTextColumn(x => x.Reference, "Reference")
                    .AddTextColumn(x => x.Description, "Description")
                    .AddDecimalColumn(x => x.Debit, "Debit")
                    .AddDecimalColumn(x => x.Credit, "Credit")
                    .AddDebitColumn(x => x.RunningBalance, "Debit Balance", x => x.AccountType)
                    .AddCreditColumn(x => x.RunningBalance, "Credit Balance", x => x.AccountType)
                    .AddTextColumn(x => x.CostCenterName, "Cost Center")
                    .HasTotals(true)
                    .Build();

                var (stream, contentType, fileName) = format.Value switch
                {
                    ReportFormat.Pdf => await _exportService.ExportToPdf(result.Data, definition),
                    ReportFormat.Excel => await _exportService.ExportToExcel(result.Data, definition),
                    ReportFormat.Csv => await _exportService.ExportToCsv(result.Data, definition),
                    _ => throw new ArgumentOutOfRangeException()
                };
                return File(stream, contentType, fileName);
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }

        [HttpGet("account-statement")]
        [HasPermission(AccountingPermissions.ReportsAccountStatement)]
        [ProducesResponseType(typeof(AccountStatementDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccountStatement(
            [FromQuery] int partnerId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] ReportFormat? format,
            CancellationToken cancellationToken)
        {
            var query = new GetAccountStatementQuery { PartnerId = partnerId, FromDate = fromDate, ToDate = toDate };
            var result = await _mediator.Send(query, cancellationToken);
            
            if (!result.Success) return BadRequest(new { result.Message });

            if (format.HasValue)
            {
                var reportName = "Account Statement";
                var definition = new ReportDefinitionBuilder<AccountStatementDto>()
                    .AddTextColumn(x => x.Date.ToString("yyyy-MM-dd"), "Date")
                    .AddTextColumn(x => x.Reference, "Reference")
                    .AddTextColumn(x => x.Description, "Description")
                    .AddDecimalColumn(x => x.Debit, "Debit")
                    .AddDecimalColumn(x => x.Credit, "Credit")
                    .AddDecimalColumn(x => x.RunningBalance, "Running Balance")
                    .HasTotals(true)
                    .Build();

                var (stream, contentType, fileName) = format.Value switch
                {
                    ReportFormat.Pdf => await _exportService.ExportToPdf(result.Data, definition),
                    ReportFormat.Excel => await _exportService.ExportToExcel(result.Data, definition),
                    ReportFormat.Csv => await _exportService.ExportToCsv(result.Data, definition),
                    _ => throw new ArgumentOutOfRangeException()
                };
                return File(stream, contentType, fileName);
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }

        [HttpGet("balance-sheet")]
        [HasPermission(AccountingPermissions.ReportsBalanceSheet)]
        [ProducesResponseType(typeof(BalanceSheetDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBalanceSheet(
            [FromQuery] DateTime asOfDate,
            [FromQuery] ReportFormat? format,
            CancellationToken cancellationToken)
        {
            var query = new GetBalanceSheetQuery { AsOfDate = asOfDate };
            var result = await _mediator.Send(query, cancellationToken);
            
            if (!result.Success) return BadRequest(new { result.Message });

            if (format.HasValue)
            {
                var reportName = "Balance Sheet";
                var definition = new ReportDefinitionBuilder<BalanceSheetItemDto>()
                    .AddTextColumn(x => x.AccountType.ToString(), "Category")
                    .AddTextColumn(x => x.AccountCode, "Account Code")
                    .AddTextColumn(x => x.AccountName, "Account Name")
                    .AddDecimalColumn(x => x.Amount, "Amount")
                    .HasTotals(false)
                    .Build();

                var (stream, contentType, fileName) = format.Value switch
                {
                    ReportFormat.Pdf => await _exportService.ExportToPdf(result.Data, definition),
                    ReportFormat.Excel => await _exportService.ExportToExcel(result.Data, definition),
                    ReportFormat.Csv => await _exportService.ExportToCsv(result.Data, definition),
                    _ => throw new ArgumentOutOfRangeException()
                };
                return File(stream, contentType, fileName);
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }

        [HttpGet("income-statement")]
        [HasPermission(AccountingPermissions.ReportsIncomeStatement)]
        [ProducesResponseType(typeof(IncomeStatementDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetIncomeStatement(
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate,
            [FromQuery] ReportFormat? format,
            CancellationToken cancellationToken)
        {
            var query = new GetIncomeStatementQuery { FromDate = fromDate, ToDate = toDate };
            var result = await _mediator.Send(query, cancellationToken);
            
            if (!result.Success) return BadRequest(new { result.Message });

            if (format.HasValue)
            {
                var reportName = "Income Statement";
                var definition = new ReportDefinitionBuilder<IncomeStatementItemDto>()
                    .AddTextColumn(x => x.AccountType.ToString(), "Category")
                    .AddTextColumn(x => x.AccountCode, "Account Code")
                    .AddTextColumn(x => x.AccountName, "Account Name")
                    .AddDecimalColumn(x => x.Amount, "Amount")
                    .HasTotals(false)
                    .Build();

                var (stream, contentType, fileName) = format.Value switch
                {
                    ReportFormat.Pdf => await _exportService.ExportToPdf(result.Data, definition),
                    ReportFormat.Excel => await _exportService.ExportToExcel(result.Data, definition),
                    ReportFormat.Csv => await _exportService.ExportToCsv(result.Data, definition),
                    _ => throw new ArgumentOutOfRangeException()
                };
                return File(stream, contentType, fileName);
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }

        [HttpGet("budget-vs-actual")]
        [HasPermission(AccountingPermissions.ReportsBudgetVsActual)]
        [ProducesResponseType(typeof(BudgetVsActualReportDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBudgetVsActual(
            [FromQuery] int budgetId,
            [FromQuery] int? costCenterId,
            [FromQuery] ReportFormat? format,
            CancellationToken cancellationToken)
        {
            var query = new GetBudgetVsActualReportQuery { BudgetId = budgetId, CostCenterId = costCenterId };
            var result = await _mediator.Send(query, cancellationToken);
            
            if (!result.Success) return BadRequest(new { result.Message });

            if (format.HasValue)
            {
                var reportName = "Budget Vs Actual";
                var definition = new ReportDefinitionBuilder<BudgetVsActualItemDto>()
                    .AddTextColumn(x => x.AccountCode, "Account Code")
                    .AddTextColumn(x => x.AccountName, "Account Name")
                    .AddDecimalColumn(x => x.PlannedAmount, "Planned")
                    .AddDecimalColumn(x => x.ActualAmount, "Actual")
                    .AddDecimalColumn(x => x.Variance, "Variance")
                    .AddPercentageColumn(x => x.UsagePercentage, "Usage (%)")
                    .HasTotals(true)
                    .Build();

                var (stream, contentType, fileName) = format.Value switch
                {
                    ReportFormat.Pdf => await _exportService.ExportToPdf(result.Data, definition),
                    ReportFormat.Excel => await _exportService.ExportToExcel(result.Data, definition),
                    ReportFormat.Csv => await _exportService.ExportToCsv(result.Data, definition),
                    _ => throw new ArgumentOutOfRangeException()
                };
                return File(stream, contentType, fileName);
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }

        [HttpGet("expense-analysis")]
        [HasPermission(AccountingPermissions.ReportsExpenseAnalysis)]
        [ProducesResponseType(typeof(ExpenseAnalysisReportDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExpenseAnalysis(
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate,
            [FromQuery] int? costCenterId,
            [FromQuery] ReportFormat? format,
            CancellationToken cancellationToken)
        {
            var query = new GetExpenseAnalysisReportQuery { FromDate = fromDate, ToDate = toDate, CostCenterId = costCenterId };
            var result = await _mediator.Send(query, cancellationToken);
            
            if (!result.Success) return BadRequest(new { result.Message });

            if (format.HasValue)
            {
                var reportName = "Expense Analysis";
                var definition = new ReportDefinitionBuilder<ExpenseAnalysisItemDto>()
                    .AddTextColumn(x => x.AccountCode, "Account Code")
                    .AddTextColumn(x => x.AccountName, "Account Name")
                    .AddTextColumn(x => x.CostCenterName, "Cost Center")
                    .AddDecimalColumn(x => x.Amount, "Amount")
                    .AddPercentageColumn(x => x.PercentageOfTotal, "% Of Total")
                    .HasTotals(true)
                    .Build();

                var (stream, contentType, fileName) = format.Value switch
                {
                    ReportFormat.Pdf => await _exportService.ExportToPdf(result.Data, definition),
                    ReportFormat.Excel => await _exportService.ExportToExcel(result.Data, definition),
                    ReportFormat.Csv => await _exportService.ExportToCsv(result.Data, definition),
                    _ => throw new ArgumentOutOfRangeException()
                };
                return File(stream, contentType, fileName);
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }


        [HttpGet("profitability")]
        [HasPermission(AccountingPermissions.ReportsProfitability)]
        [ProducesResponseType(typeof(ProfitabilityReportDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfitability(
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate,
            [FromQuery] int? costCenterId,
            [FromQuery] ReportFormat? format,
            CancellationToken cancellationToken)
        {
            var query = new GetProfitabilityReportQuery { FromDate = fromDate, ToDate = toDate, CostCenterId = costCenterId };
            var result = await _mediator.Send(query, cancellationToken);
            
            if (!result.Success) return BadRequest(new { result.Message });

            if (format.HasValue)
            {
                var reportName = "Profitability";
                var definition = new ReportDefinitionBuilder<ProfitabilityItemDto>()
                    .AddTextColumn(x => x.CostCenterName, "Cost Center")
                    .AddDecimalColumn(x => x.Revenue, "Revenue")
                    .AddDecimalColumn(x => x.Expenses, "Expenses")
                    .AddDecimalColumn(x => x.Profit, "Profit")
                    .AddPercentageColumn(x => x.ProfitMargin, "Profit Margin")
                    .HasTotals(true)
                    .Build();

                var (stream, contentType, fileName) = format.Value switch
                {
                    ReportFormat.Pdf => await _exportService.ExportToPdf(result.Data, definition),
                    ReportFormat.Excel => await _exportService.ExportToExcel(result.Data, definition),
                    ReportFormat.Csv => await _exportService.ExportToCsv(result.Data, definition),
                    _ => throw new ArgumentOutOfRangeException()
                };
                return File(stream, contentType, fileName);
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }
    }
}
