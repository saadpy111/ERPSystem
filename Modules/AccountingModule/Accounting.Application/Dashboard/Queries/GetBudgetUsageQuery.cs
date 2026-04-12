using Accounting.Application.Common.Models;
using Accounting.Application.Dashboard.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Accounting.Application.Dashboard.Queries
{
    public class GetBudgetUsageQuery : IRequest<Result<IEnumerable<BudgetUsageDto>>>
    {
        public int BudgetId { get; set; }
    }
}
