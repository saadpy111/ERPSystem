using Accounting.Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.CashAccounts.Commands.DeleteCashAccount
{
    public class DeleteCashAccountCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteCashAccountCommandHandler : IRequestHandler<DeleteCashAccountCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCashAccountCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteCashAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.CashAccounts.GetByIdAsync(request.Id);
            if (entity == null) return Result.Failure("Cash account not found.");

            entity.IsDeleted = true;
            _unitOfWork.CashAccounts.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok("Cash account deleted successfully.");
        }
    }
}
