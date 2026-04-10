using Accounting.Application.Common.Models;
using Accounting.Application.Features.Budgets.DTOs;
using Accounting.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.Budgets.Queries.GetBudgetById
{
    public class GetBudgetByIdQuery : IRequest<Result<BudgetDto>>
    {
        public int Id { get; set; }
    }

    public class GetBudgetByIdQueryHandler : IRequestHandler<GetBudgetByIdQuery, Result<BudgetDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBudgetByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<BudgetDto>> Handle(GetBudgetByIdQuery request, CancellationToken cancellationToken)
        {
            var budget = await _unitOfWork.Budgets.Query()
                .AsNoTracking()
                .Include(b => b.Lines)
                    .ThenInclude(l => l.Account)
                .Include(b => b.Lines)
                    .ThenInclude(l => l.CostCenter)
                .Include(b => b.FiscalYear)
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (budget == null)
                return Result<BudgetDto>.Failure("Budget not found.");

            var dto = new BudgetDto
            {
                Id = budget.Id,
                Name = budget.Name,
                FiscalYearId = budget.FiscalYearId,
                FiscalYearName = budget.FiscalYear.Name,
                Status = budget.Status,
                EnforceBudgetControl = budget.EnforceBudgetControl,
                Lines = budget.Lines.Select(l => new BudgetLineDto
                {
                    Id = l.Id,
                    AccountId = l.AccountId,
                    AccountName = l.Account.NameEn ?? l.Account.NameAr,
                    CostCenterId = l.CostCenterId,
                    CostCenterName = l.CostCenter?.NameEn ?? l.CostCenter?.NameAr,
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    PlannedAmount = l.PlannedAmount
                }).ToList()
            };

            return Result<BudgetDto>.Ok(dto);
        }
    }
}
