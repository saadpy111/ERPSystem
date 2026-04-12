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
                var (stream, contentType, fileName) = format.Value switch
                {
                    ReportFormat.Pdf => await _exportService.ExportToPdf(result.Data, reportName),
                    ReportFormat.Excel => await _exportService.ExportToExcel(result.Data, reportName),
                    ReportFormat.Csv => await _exportService.ExportToCsv(result.Data, reportName),
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
                var (stream, contentType, fileName) = format.Value switch
                {
                    ReportFormat.Pdf => await _exportService.ExportToPdf(result.Data, reportName),
                    ReportFormat.Excel => await _exportService.ExportToExcel(result.Data, reportName),
                    ReportFormat.Csv => await _exportService.ExportToCsv(result.Data, reportName),
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
                var (stream, contentType, fileName) = format.Value switch
                {
                    ReportFormat.Pdf => await _exportService.ExportToPdf(result.Data, reportName),
                    ReportFormat.Excel => await _exportService.ExportToExcel(result.Data, reportName),
                    ReportFormat.Csv => await _exportService.ExportToCsv(result.Data, reportName),
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
                var (stream, contentType, fileName) = format.Value switch
                {
                    ReportFormat.Pdf => await _exportService.ExportToPdf(result.Data, reportName),
                    ReportFormat.Excel => await _exportService.ExportToExcel(result.Data, reportName),
                    ReportFormat.Csv => await _exportService.ExportToCsv(result.Data, reportName),
                    _ => throw new ArgumentOutOfRangeException()
                };

                return File(stream, contentType, fileName);
            }

            return Ok(new { success = true, data = result.Data });
        }
    }
}
