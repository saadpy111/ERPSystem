using Accounting.Application.Reports.DTOs;
using MediatR;
using System;

namespace Accounting.Application.Reports.Queries.GetBalanceSheet
{
    /// <summary>
    /// Returns a Balance Sheet (Assets / Liabilities / Equity) as of a specific date.
    /// Only Posted journal entries whose date is on or before <see cref="AsOfDate"/> are included.
    /// </summary>
    public class GetBalanceSheetQuery : IRequest<BalanceSheetDto>
    {
        /// <summary>Cut-off date for the report (inclusive).</summary>
        public DateTime AsOfDate { get; set; }
    }
}
