using System;
using Accounting.Application.Common.Models;
using Accounting.Application.Dashboard.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Accounting.Application.Dashboard.Queries
{
    public class GetMonthlyOverviewQuery : IRequest<Result<IEnumerable<MonthlyOverviewDto>>>
    {
        public int Year { get; set; } = DateTime.UtcNow.Year;
    }
}
