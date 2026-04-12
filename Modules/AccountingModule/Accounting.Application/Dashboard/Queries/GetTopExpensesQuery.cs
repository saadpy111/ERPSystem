using Accounting.Application.Common.Models;
using Accounting.Application.Dashboard.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Accounting.Application.Dashboard.Queries
{
    public class GetTopExpensesQuery : IRequest<Result<IEnumerable<TopExpenseDto>>>
    {
        public int NumberOfRecords { get; set; } = 5;
    }
}
