using Accounting.Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.CashAccounts.Commands.DeleteCashAccount
{
    public class DeleteCashAccountCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteCashAccountCommandHandler : IRequestHandler<DeleteCashAccountCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCashAccountCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCashAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.CashAccounts.GetByIdAsync(request.Id);
            if (entity == null)
                return false;

            entity.IsDeleted = true;
            _unitOfWork.CashAccounts.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
