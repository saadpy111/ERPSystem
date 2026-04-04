using Accounting.Application.Reports.DTOs;
using MediatR;
using System;
using System.Collections.Generic;

namespace Accounting.Application.Reports.Queries.GetGeneralLedger
{
    public class GetGeneralLedgerQuery : IRequest<List<LedgerDto>>
    {
        public int AccountId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
