using Accounting.Application.Common.Models;
using Accounting.Application.Reports.DTOs;
using MediatR;
using System;

namespace Accounting.Application.Reports.Queries
{
    public class GetBudgetVsActualReportQuery : IRequest<Result<Accounting.Application.Reports.Models.ReportResponse<BudgetVsActualItemDto>>>
    {
        public int BudgetId { get; set; }
        public int? CostCenterId { get; set; }
    }
}
