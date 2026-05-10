using Accounting.Application.Reports.DTOs;
using MediatR;
using Accounting.Application.Common.Models;
using System;

namespace Accounting.Application.Reports.Queries.GetBalanceSheet
{
    /// <summary>
    /// Returns a Balance Sheet (Assets / Liabilities / Equity) as of a specific date.
    /// Only Posted journal entries whose date is on or before <see cref="AsOfDate"/> are included.
    /// </summary>
    public class GetBalanceSheetQuery : IRequest<Result<Accounting.Application.Reports.Models.ReportResponse<BalanceSheetItemDto>>>
    {
        /// <summary>Cut-off date for the report (inclusive).</summary>
        public DateTime AsOfDate { get; set; }
        public int? CostCenterId { get; set; }
    }
}
