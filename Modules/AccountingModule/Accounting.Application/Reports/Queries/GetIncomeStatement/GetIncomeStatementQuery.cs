using Accounting.Application.Reports.DTOs;
using MediatR;
using System;

namespace Accounting.Application.Reports.Queries.GetIncomeStatement
{
    public class GetIncomeStatementQuery : IRequest<IncomeStatementDto>
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
