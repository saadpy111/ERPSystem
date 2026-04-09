using Accounting.Application.Reports.DTOs;
using MediatR;
using Accounting.Application.Common.Models;
using System;
using System.Collections.Generic;

namespace Accounting.Application.Reports.Queries.GetAccountStatement
{
    public class GetAccountStatementQuery : IRequest<Result<List<AccountStatementDto>>>
    {
        public int PartnerId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
