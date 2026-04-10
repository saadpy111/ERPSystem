using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.CostCenters.Commands.DeleteCostCenter
{
    public class DeleteCostCenterCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteCostCenterCommandHandler : IRequestHandler<DeleteCostCenterCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCostCenterCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteCostCenterCommand request, CancellationToken cancellationToken)
        {
            var cc = await _unitOfWork.CostCenters.GetByIdAsync(request.Id);
            if (cc == null) return Result<int>.Failure("Cost Center not found.");

            var hasTransactions = await _unitOfWork.CostCenters.HasTransactionsAsync(request.Id);
            
            // Following user requirement: "Disable (IsActive = false) instead of delete"
            // "Do NOT allow deletion at all if CostCenter is used in transactions"
            cc.IsActive = false;
            
            if (!hasTransactions)
            {
                // If not used in transactions, we could potentially soft delete (IsDeleted = true)
                // but the user emphasized IsActive flag.
                cc.IsDeleted = true; 
            }

            _unitOfWork.CostCenters.Update(cc);
            await _unitOfWork.SaveChangesAsync();

            return Result<int>.Ok(cc.Id, hasTransactions 
                ? "Cost Center deactivated because it has transactions." 
                : "Cost Center deleted successfully.");
        }
    }
}
