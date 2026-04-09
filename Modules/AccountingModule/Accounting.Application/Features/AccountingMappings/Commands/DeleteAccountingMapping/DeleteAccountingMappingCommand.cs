using Accounting.Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.AccountingMappings.Commands.DeleteAccountingMapping
{
    public class DeleteAccountingMappingCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }

    public class DeleteAccountingMappingCommandHandler : IRequestHandler<DeleteAccountingMappingCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAccountingMappingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteAccountingMappingCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.AccountingMappings.GetByIdAsync(request.Id);
            if (entity == null)
                return false;

            entity.IsDeleted = true;
            _unitOfWork.AccountingMappings.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
