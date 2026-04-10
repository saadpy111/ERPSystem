using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.Budgets.Commands.UpdateBudget
{
    public class UpdateBudgetCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool EnforceBudgetControl { get; set; }
        public List<BudgetLineUpdateRequest> Lines { get; set; } = new();
    }

    public class BudgetLineUpdateRequest
    {
        public int? Id { get; set; } // Null for new lines
        public int AccountId { get; set; }
        public int? CostCenterId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PlannedAmount { get; set; }
    }

    public class UpdateBudgetCommandHandler : IRequestHandler<UpdateBudgetCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBudgetCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateBudgetCommand request, CancellationToken cancellationToken)
        {
            var budget = await _unitOfWork.Budgets.Query()
                .Include(b => b.Lines)
                .Include(b => b.FiscalYear)
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (budget == null)
                return Result.Failure("Budget not found.");

            if (budget.Status == BudgetStatus.Closed)
                return Result.Failure("Closed budgets cannot be updated.");

            budget.Name = request.Name;
            budget.EnforceBudgetControl = request.EnforceBudgetControl;

            var fiscalYear = budget.FiscalYear;

            // Remove lines not in the request
            var requestLineIds = request.Lines.Where(l => l.Id.HasValue).Select(l => l.Id!.Value).ToList();
            var linesToRemove = budget.Lines.Where(l => !requestLineIds.Contains(l.Id)).ToList();
            foreach (var line in linesToRemove)
            {
                budget.Lines.Remove(line);
                // Since it's a collection, removing might not delete from DB if not configured for orphan removal.
                // We'll manage it via UoW if needed, but usually .Lines.Remove is enough if EfCore is configured.
            }

            foreach (var lineReq in request.Lines)
            {
                // Validation
                if (lineReq.StartDate < fiscalYear.StartDate || lineReq.EndDate > fiscalYear.EndDate)
                {
                    return Result.Failure($"Budget line for Account {lineReq.AccountId} is outside the fiscal year boundaries.");
                }

                if (lineReq.StartDate > lineReq.EndDate)
                {
                    return Result.Failure($"Start date cannot be after end date for Account {lineReq.AccountId}.");
                }

                // Overlap check (within the request)
                var overlap = request.Lines.Any(l => 
                    l != lineReq &&
                    l.AccountId == lineReq.AccountId && 
                    l.CostCenterId == lineReq.CostCenterId && 
                    lineReq.StartDate < l.EndDate && l.StartDate < lineReq.EndDate);

                if (overlap)
                {
                    return Result.Failure($"Overlapping periods detected for Account {lineReq.AccountId} and Cost Center {lineReq.CostCenterId}.");
                }

                if (lineReq.Id.HasValue)
                {
                    var existingLine = budget.Lines.FirstOrDefault(l => l.Id == lineReq.Id.Value);
                    if (existingLine != null)
                    {
                        existingLine.AccountId = lineReq.AccountId;
                        existingLine.CostCenterId = lineReq.CostCenterId;
                        existingLine.StartDate = lineReq.StartDate;
                        existingLine.EndDate = lineReq.EndDate;
                        existingLine.PlannedAmount = lineReq.PlannedAmount;
                    }
                }
                else
                {
                    budget.Lines.Add(new BudgetLine
                    {
                        AccountId = lineReq.AccountId,
                        CostCenterId = lineReq.CostCenterId,
                        StartDate = lineReq.StartDate,
                        EndDate = lineReq.EndDate,
                        PlannedAmount = lineReq.PlannedAmount,
                        Budget = budget
                    });
                }
            }

              _unitOfWork.Budgets.Update(budget);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok("Budget updated successfully.");
        }
    }
}
