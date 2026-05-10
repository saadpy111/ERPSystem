using Accounting.Application.Common.Models;
using Accounting.Application.Reports.DTOs;
using MediatR;
using System;

namespace Accounting.Application.Reports.Queries
{
    public class GetExpenseAnalysisReportQuery : IRequest<Result<Accounting.Application.Reports.Models.ReportResponse<ExpenseAnalysisItemDto>>>
    {
        public int? CostCenterId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
