using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.Budgets.Commands.CreateBudget
{
    public class CreateBudgetCommand : IRequest<Result<int>>
    {
        public string Name { get; set; } = null!;
        public int FiscalYearId { get; set; }
        public bool EnforceBudgetControl { get; set; }
        public List<BudgetLineRequest> Lines { get; set; } = new();
    }

    public class BudgetLineRequest
    {
        public int AccountId { get; set; }
        public int? CostCenterId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PlannedAmount { get; set; }
    }

    public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBudgetCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
        {
            // 1. Validate Fiscal Year
            var fiscalYear = await _unitOfWork.FiscalYears.GetByIdAsync(request.FiscalYearId);
            if (fiscalYear == null)
                return Result<int>.Failure("Fiscal year not found.");

            var budget = new Budget
            {
                Name = request.Name,
                FiscalYearId = request.FiscalYearId,
                Status = BudgetStatus.Draft,
                EnforceBudgetControl = request.EnforceBudgetControl,
                TenantId = Guid.Empty // Will be set by Interceptor if implemented, or manually if needed
            };

            var lines = new List<BudgetLine>();

            foreach (var lineReq in request.Lines)
            {
                // 2. Validate Fiscal Year Alignment
                if (lineReq.StartDate < fiscalYear.StartDate || lineReq.EndDate > fiscalYear.EndDate)
                {
                    return Result<int>.Failure($"Budget line for Account {lineReq.AccountId} is outside the fiscal year boundaries ({fiscalYear.StartDate:yyyy-MM-dd} to {fiscalYear.EndDate:yyyy-MM-dd}).");
                }

                if (lineReq.StartDate > lineReq.EndDate)
                {
                    return Result<int>.Failure($"Start date cannot be after end date for Account {lineReq.AccountId}.");
                }

                // 3. Validate Overlapping Periods (within the same request first)
                var overlap = lines.Any(l => 
                    l.AccountId == lineReq.AccountId && 
                    l.CostCenterId == lineReq.CostCenterId && 
                    lineReq.StartDate < l.EndDate && l.StartDate < lineReq.EndDate);

                if (overlap)
                {
                    return Result<int>.Failure($"Overlapping periods detected for Account {lineReq.AccountId} and Cost Center {lineReq.CostCenterId} within the same budget.");
                }

                lines.Add(new BudgetLine
                {
                    AccountId = lineReq.AccountId,
                    CostCenterId = lineReq.CostCenterId,
                    StartDate = lineReq.StartDate,
                    EndDate = lineReq.EndDate,
                    PlannedAmount = lineReq.PlannedAmount,
                    Budget = budget
                });
            }

            budget.Lines = lines;

            await _unitOfWork.Budgets.AddAsync(budget);
            await _unitOfWork.SaveChangesAsync();

            return Result<int>.Ok(budget.Id, "Budget created successfully.");
        }
    }
}

