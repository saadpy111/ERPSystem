using Accounting.Application.Reports.DTOs;
using MediatR;
using Accounting.Application.Common.Models;
using System;
using System.Collections.Generic;

namespace Accounting.Application.Reports.Queries.GetTrialBalance
{
    public class GetTrialBalanceQuery : IRequest<Result<Accounting.Application.Reports.Models.ReportResponse<TrialBalanceDto>>>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? CostCenterId { get; set; }
    }
}
