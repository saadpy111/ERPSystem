using Accounting.Application.Reports.DTOs;
using MediatR;
using System;
using System.Collections.Generic;

namespace Accounting.Application.Reports.Queries.GetAccountStatement
{
    public class GetAccountStatementQuery : IRequest<List<AccountStatementDto>>
    {
        public int PartnerId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
