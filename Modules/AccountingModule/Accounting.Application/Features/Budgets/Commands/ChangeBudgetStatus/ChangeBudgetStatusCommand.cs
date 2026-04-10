using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.Budgets.Commands.ChangeBudgetStatus
{
    public class ChangeBudgetStatusCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public BudgetStatus NewStatus { get; set; }
    }

    public class ChangeBudgetStatusCommandHandler : IRequestHandler<ChangeBudgetStatusCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangeBudgetStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ChangeBudgetStatusCommand request, CancellationToken cancellationToken)
        {
            var budget = await _unitOfWork.Budgets.GetByIdAsync(request.Id);

            if (budget == null)
                return Result.Failure("Budget not found.");

            // Transition validation
            if (budget.Status == BudgetStatus.Closed && request.NewStatus != BudgetStatus.Closed)
                return Result.Failure("Closed budgets cannot be reopened.");

            budget.Status = request.NewStatus;

              _unitOfWork.Budgets.Update(budget);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok($"Budget status changed to {request.NewStatus}.");
        }
    }
}
