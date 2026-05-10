using System;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Dashboard.Queries;
using Accounting.Application.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Core.Constants.Permissions;
using Accounting.Application.Interfaces.Services;
using Accounting.Application.Reports.Models;
using Accounting.Application.Reports.Builders;
using System.Linq;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/dashboard")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Accounting")]
    [HasPermission(AccountingPermissions.DashboardView)]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IReportExportService _exportService;

        public DashboardController(IMediator mediator, IReportExportService exportService)
        {
            _mediator = mediator;
            _exportService = exportService;
        }

        [HttpGet("profit-by-costcenter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfitByCostCenter(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] ReportFormat? format,
            CancellationToken cancellationToken)
        {
            var query = new GetProfitByCostCenterQuery { FromDate = fromDate, ToDate = toDate };
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            if (format.HasValue)
            {
                var reportName = "Profit By Cost Center";
                var response = new ReportResponse<Accounting.Application.Dashboard.DTOs.ProfitByCostCenterDto>
                {
                    Items = result.Data.ToList(),
                    Metadata = new ReportMetadata { ReportName = reportName, FromDate = fromDate, ToDate = toDate }
                };

                var definition = new ReportDefinitionBuilder<Accounting.Application.Dashboard.DTOs.ProfitByCostCenterDto>()
                    .AddTextColumn(x => x.CostCenterName, "Cost Center")
                    .AddDecimalColumn(x => x.Revenue, "Revenue")
                    .AddDecimalColumn(x => x.Expenses, "Expenses")
                    .AddDecimalColumn(x => x.Profit, "Profit")
                    .HasTotals(true)
                    .Build();

                var (stream, contentType, fileName) = format.Value switch
                {
                    ReportFormat.Pdf => await _exportService.ExportToPdf(response, definition),
                    ReportFormat.Excel => await _exportService.ExportToExcel(response, definition),
                    ReportFormat.Csv => await _exportService.ExportToCsv(response, definition),
                    _ => throw new ArgumentOutOfRangeException()
                };

                return File(stream, contentType, fileName);
            }

            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("budget-usage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBudgetUsage(
            [FromQuery] int budgetId,
            [FromQuery] ReportFormat? format,
            CancellationToken cancellationToken)
        {
            var query = new GetBudgetUsageQuery { BudgetId = budgetId };
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            if (format.HasValue)
            {
                var reportName = "Budget Usage";
                var response = new ReportResponse<Accounting.Application.Dashboard.DTOs.BudgetUsageDto>
                {
                    Items = result.Data.ToList(),
                    Metadata = new ReportMetadata { ReportName = reportName }
                };

                var definition = new ReportDefinitionBuilder<Accounting.Application.Dashboard.DTOs.BudgetUsageDto>()
                    .AddTextColumn(x => x.BudgetName, "Budget")
                    .AddTextColumn(x => x.CostCenterName, "Cost Center")
                    .AddDecimalColumn(x => x.PlannedAmount, "Planned")
                    .AddDecimalColumn(x => x.ActualAmount, "Actual")
                    .AddDecimalColumn(x => x.PlannedAmount - x.ActualAmount, "Variance")
                    .AddPercentageColumn(x => x.UsagePercentage, "Usage (%)")
                    .HasTotals(true)
                    .Build();

                var (stream, contentType, fileName) = format.Value switch
                {
                    ReportFormat.Pdf => await _exportService.ExportToPdf(response, definition),
                    ReportFormat.Excel => await _exportService.ExportToExcel(response, definition),
                    ReportFormat.Csv => await _exportService.ExportToCsv(response, definition),
                    _ => throw new ArgumentOutOfRangeException()
                };

                return File(stream, contentType, fileName);
            }

            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("top-expenses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTopExpenses(
            [FromQuery] int numberOfRecords = 5,
            [FromQuery] ReportFormat? format = null,
            CancellationToken cancellationToken = default)
        {
            var query = new GetTopExpensesQuery { NumberOfRecords = numberOfRecords };
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            if (format.HasValue)
            {
                var reportName = "Top Expenses";
                var response = new ReportResponse<Accounting.Application.Dashboard.DTOs.TopExpenseDto>
                {
                    Items = result.Data.ToList(),
                    Metadata = new ReportMetadata { ReportName = reportName }
                };

                var definition = new ReportDefinitionBuilder<Accounting.Application.Dashboard.DTOs.TopExpenseDto>()
                    .AddTextColumn(x => x.AccountName, "Expense Account")
                    .AddDecimalColumn(x => x.TotalExpense, "Total Amount")
                    .HasTotals(true)
                    .Build();

                var (stream, contentType, fileName) = format.Value switch
                {
                    ReportFormat.Pdf => await _exportService.ExportToPdf(response, definition),
                    ReportFormat.Excel => await _exportService.ExportToExcel(response, definition),
                    ReportFormat.Csv => await _exportService.ExportToCsv(response, definition),
                    _ => throw new ArgumentOutOfRangeException()
                };

                return File(stream, contentType, fileName);
            }

            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("monthly-overview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMonthlyOverview(
            [FromQuery] int? year,
            [FromQuery] ReportFormat? format,
            CancellationToken cancellationToken)
        {
            var query = new GetMonthlyOverviewQuery { Year = year ?? DateTime.UtcNow.Year };
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            if (format.HasValue)
            {
                var reportName = "Monthly Overview";
                var response = new ReportResponse<Accounting.Application.Dashboard.DTOs.MonthlyOverviewDto>
                {
                    Items = result.Data.ToList(),
                    Metadata = new ReportMetadata { ReportName = reportName }
                };

                var definition = new ReportDefinitionBuilder<Accounting.Application.Dashboard.DTOs.MonthlyOverviewDto>()
                    .AddTextColumn(x => x.MonthName, "Month")
                    .AddDecimalColumn(x => x.Revenue, "Revenue")
                    .AddDecimalColumn(x => x.Expenses, "Expenses")
                    .AddDecimalColumn(x => x.Profit, "Net Profit")
                    .HasTotals(true)
                    .Build();

                var (stream, contentType, fileName) = format.Value switch
                {
                    ReportFormat.Pdf => await _exportService.ExportToPdf(response, definition),
                    ReportFormat.Excel => await _exportService.ExportToExcel(response, definition),
                    ReportFormat.Csv => await _exportService.ExportToCsv(response, definition),
                    _ => throw new ArgumentOutOfRangeException()
                };

                return File(stream, contentType, fileName);
            }

            return Ok(new { success = true, data = result.Data });
        }
    }
}
