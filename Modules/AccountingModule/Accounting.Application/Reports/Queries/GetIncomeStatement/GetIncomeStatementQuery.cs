using Accounting.Application.Reports.DTOs;
using MediatR;
using Accounting.Application.Common.Models;
using System;

namespace Accounting.Application.Reports.Queries.GetIncomeStatement
{
    public class GetIncomeStatementQuery : IRequest<Result<Accounting.Application.Reports.Models.ReportResponse<IncomeStatementItemDto>>>
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? CostCenterId { get; set; }
        public bool GroupByCostCenter { get; set; }
    }
}
