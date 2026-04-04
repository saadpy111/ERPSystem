using Accounting.Application.Reports.DTOs;
using MediatR;
using System;
using System.Collections.Generic;

namespace Accounting.Application.Reports.Queries.GetTrialBalance
{
    public class GetTrialBalanceQuery : IRequest<List<TrialBalanceDto>>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
