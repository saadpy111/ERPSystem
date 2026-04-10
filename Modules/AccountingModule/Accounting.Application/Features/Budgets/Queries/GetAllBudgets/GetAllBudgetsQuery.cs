using Accounting.Application.Common.Models;
using Accounting.Application.Features.Budgets.DTOs;
using Accounting.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.Budgets.Queries.GetAllBudgets
{
    public class GetAllBudgetsQuery : IRequest<Result<List<BudgetDto>>>
    {
        public int? FiscalYearId { get; set; }
    }

    public class GetAllBudgetsQueryHandler : IRequestHandler<GetAllBudgetsQuery, Result<List<BudgetDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllBudgetsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<BudgetDto>>> Handle(GetAllBudgetsQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Budgets.Query()
                .AsNoTracking()
                .Include(b => b.FiscalYear)
                .AsQueryable();

            if (request.FiscalYearId.HasValue)
            {
                query = query.Where(b => b.FiscalYearId == request.FiscalYearId.Value);
            }

            var budgets = await query.ToListAsync(cancellationToken);

            var dtos = budgets.Select(b => new BudgetDto
            {
                Id = b.Id,
                Name = b.Name,
                FiscalYearId = b.FiscalYearId,
                FiscalYearName = b.FiscalYear.Name,
                Status = b.Status,
                EnforceBudgetControl = b.EnforceBudgetControl
            }).ToList();

            return Result<List<BudgetDto>>.Ok(dtos);
        }
    }
}
