using Accounting.Application.Common.Models;
using Accounting.Application.Dashboard.DTOs;
using MediatR;
using System;
using System.Collections.Generic;

namespace Accounting.Application.Dashboard.Queries
{
    public class GetProfitByCostCenterQuery : IRequest<Result<IEnumerable<ProfitByCostCenterDto>>>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
